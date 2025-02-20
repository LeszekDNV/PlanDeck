using PlanDeck.Application.Interfaces;
using PlanDeck.Contracts.Dtos;
using PlanDeck.Contracts.Room.Create;
using PlanDeck.Contracts.Room.Get;
using PlanDeck.Contracts.Room.State;
using PlanDeck.Contracts.Room.Update;
using PlanDeck.Domain.Entities;
using PlanDeck.Domain.Entities.Enums;
using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using System.Threading.Channels;

namespace PlanDeck.Application.Services;

public class RoomService(IKeyRepository<Room, Guid> roomRepository, IKeyRepository<Participant, Guid> participantRepository) : IRoomService
{
    private static readonly ConcurrentDictionary<string, List<Channel<ServerStreamMessage>>> _roomChannels = new();

    public async Task<Guid> CreateRoomAsync(CreateRoomRequest request)
    {
        VotingSystem votingSystem = (VotingSystem)request.VotingSystem;
        RoomPermission whoCanReveal = (RoomPermission)request.WhoCanRevealCards;
        RoomPermission whoCanManage = (RoomPermission)request.WhoCanManageIssues;

        Room newRoom = new()
        {
            Name = request.Name,
            VotingSystem = votingSystem,
            WhoCanRevealCards = whoCanReveal,
            WhoCanManageIssues = whoCanManage,
            AutoRevealCards = request.AutoRevealCards,
            ShowAverage = request.ShowAverage            
        };

        if (Guid.TryParse(request.Owner.Id, out Guid clientId))
        {
            newRoom.OwnerClientId = clientId;
        }

        await roomRepository.AddAsync(newRoom);
        return newRoom.Id;
    }

    public async Task<UpdateRoomResponse> UpdateRoomAsync(UpdateRoomRequest request)
    {
        if (!Guid.TryParse(request.Id, out Guid roomId)) return new UpdateRoomResponse(false);
        if (!roomRepository.GetAll().Any(r => r.Id == roomId)) return new UpdateRoomResponse(false);

        VotingSystem votingSystem = (VotingSystem)request.RoomSettings.VotingSystem;
        RoomPermission whoCanReveal = (RoomPermission)request.RoomSettings.WhoCanRevealCards;
        RoomPermission whoCanManage = (RoomPermission)request.RoomSettings.WhoCanManageIssues;
        Room room = new()
        {
            Id = roomId,
            Name = request.RoomSettings.Name,
            VotingSystem = votingSystem,
            WhoCanRevealCards = whoCanReveal,
            WhoCanManageIssues = whoCanManage,
            AutoRevealCards = request.RoomSettings.AutoRevealCards,
            ShowAverage = request.RoomSettings.ShowAverage            
        };

        if (!string.IsNullOrEmpty(request.RoomSettings.Owner?.Id) && Guid.TryParse(request.RoomSettings.Owner?.Id, out Guid ownerId))
        {
            room.OwnerClientId = ownerId;
        }

        await roomRepository.UpdateAsync(room);

        BroadcastToRoom(room.Id.ToString(), new ServerStreamMessage 
        {
            EventType = RoomEventTypes.SETTINGS_CHANGED,
            RoomSettings = new()
            {
                Name = room.Name,
                AutoRevealCards = room.AutoRevealCards,
                ShowAverage = room.ShowAverage,
                VotingSystem = (VotingSystemsDto)room.VotingSystem,
                WhoCanManageIssues = (RoomPermissionsDto)room.WhoCanManageIssues,
                WhoCanRevealCards = (RoomPermissionsDto)room.WhoCanRevealCards,
                Owner = request.RoomSettings.Owner,
            },
            RoomId = room.Id.ToString(),
            User = request.User,
        });

        return new UpdateRoomResponse(true)
        {
            RoomSettings = new RoomSettingsDto
            {
                Name = room.Name,
                AutoRevealCards = room.AutoRevealCards,
                VotingSystem = (VotingSystemsDto)room.VotingSystem,
                WhoCanManageIssues = (RoomPermissionsDto)room.WhoCanManageIssues,
                WhoCanRevealCards = (RoomPermissionsDto)room.WhoCanRevealCards,
                ShowAverage = room.ShowAverage
            }
        };
    }

    public async Task<GetRoomSettingsResponse> GetRoomSettings(GetRoomSettingsRequest request)
    {
        GetRoomSettingsResponse response = new();

        if (!Guid.TryParse(request.Id, out Guid id))
            return response;

        Room? room = await roomRepository.GetById(id, r => r.Participants);
        if (room is null) return response;



        response.RoomSettings = new RoomSettingsDto
        {
            Name = room.Name,
            AutoRevealCards = room.AutoRevealCards,
            VotingSystem = (VotingSystemsDto)room.VotingSystem,
            WhoCanManageIssues = (RoomPermissionsDto)room.WhoCanManageIssues,
            WhoCanRevealCards = (RoomPermissionsDto)room.WhoCanRevealCards,
            ShowAverage = room.ShowAverage
        };

        if (room.OwnerClientId != null)
        {
            Participant? owner = room.Participants.FirstOrDefault(p => p.ClientUserId == room.OwnerClientId);
            if (owner is not null)
            {
                response.RoomSettings.Owner = new UserDto
                {
                    Id = owner.ClientUserId.ToString(),
                    Name = owner.Name,
                    LastPlanningRoom = room.Id.ToString()
                };
            };
        }

        return response;
    }

    /// <summary>
    /// Create bidirectional connection with client and server.
    /// </summary>
    /// <param name="requestStream"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public IAsyncEnumerable<ServerStreamMessage> Connect(SubscribeRequest request, CancellationToken cancellationToken)
    {
        Channel<ServerStreamMessage> channel = Channel.CreateUnbounded<ServerStreamMessage>();  // Kanał do wysyłania wiadomości do klienta

        List<Channel<ServerStreamMessage>> list = _roomChannels.GetOrAdd(request.RoomId, _ => []);
        lock (list)
        {
            list.Add(channel);
        }

        return ReadEventsAsync(channel, cancellationToken);
    }

    private async IAsyncEnumerable<ServerStreamMessage> ReadEventsAsync(Channel<ServerStreamMessage> channel, [EnumeratorCancellation] CancellationToken ct)
    {
        // Dopóki klient nie przerwie, odczytujemy eventy z kanału i yield-ujemy
        while (!ct.IsCancellationRequested)
        {
            ServerStreamMessage ev;
            try
            {
                ev = await channel.Reader.ReadAsync(ct);
            }
            catch
            {
                break;
            }
            yield return ev;
        }
    }

    private async Task UpdateUserLastSeen(string userId, string roomId)
    {
        if (Guid.TryParse(userId, out var user) && Guid.TryParse(roomId, out Guid room))
        {
            var participant = participantRepository.GetAll().FirstOrDefault(p => p.ClientUserId == user && p.RoomId == room);
            if (participant != null)
            {
                participant.LastSeen = DateTime.UtcNow;

                await participantRepository.ApplyPatchAsync(participant, 
                    new Domain.ValueObjects.Patch 
                    { 
                        PropertyName = nameof(Participant.LastSeen), 
                        PropertyValue = DateTime.UtcNow 
                    });
            }
        }
    }

    private void BroadcastToRoom(string roomId, ServerStreamMessage message)
    {
        if (_roomChannels.TryGetValue(roomId, out List<Channel<ServerStreamMessage>>? subs))
        {
            lock (subs)
            {
                foreach (Channel<ServerStreamMessage> ch in subs)
                {
                    ch.Writer.TryWrite(message);
                }
            }
        }
    }
}

