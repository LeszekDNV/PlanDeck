using PlanDeck.Application.Interfaces;
using PlanDeck.Contracts.Dtos;
using PlanDeck.Contracts.Room;
using PlanDeck.Contracts.Room.Create;
using PlanDeck.Contracts.Room.Get;
using PlanDeck.Contracts.Room.Update;
using PlanDeck.Domain.Entities;
using PlanDeck.Domain.Entities.Enums;

namespace PlanDeck.Application.Services;

public class RoomService(IKeyRepository<Room, Guid> roomRepository) : IRoomService
{
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
            ShowAverage = request.ShowAverage,
        };

        await roomRepository.AddAsync(newRoom);
        return newRoom.Id;
    }

    public async Task<UpdateRoomResponse> UpdateRoomAsync(UpdateRoomRequest request)
    {
        if (!Guid.TryParse(request.Id, out Guid roomId)) return new UpdateRoomResponse(false);

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
            ShowAverage = request.RoomSettings.ShowAverage,
        };
        await roomRepository.UpdateAsync(room);
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

        Room? room = await roomRepository.GetById(id);
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
        return response;
    }
}

