using PlanDeck.Application.Interfaces;
using PlanDeck.Contracts.Room;
using PlanDeck.Contracts.Room.Create;
using PlanDeck.Domain.Entities;
using ProtoBuf.Grpc;

namespace PlanDeck.App.GrpcServices;

public class GrpcRoomService(IKeyRepository<Room, Guid> roomRepository) : IGrpcRoomService
{
    public async Task<CreateRoomResponse> CreateRoom(CreateRoomRequest request, CallContext context = default)
    {
        Room room = new()
        {
            Name = request.Name,
            VotingSystem = (Domain.Entities.Enums.VotingSystem)(int)request.VotingSystem,
            WhoCanRevealCards = (Domain.Entities.Enums.RoomPermission)(int)request.WhoCanRevealCards,
            WhoCanManageIssues = (Domain.Entities.Enums.RoomPermission)(int)request.WhoCanManageIssues,
            AutoRevealCards = request.AutoRevealCards,
            ShowAverage = request.ShowAverage,
            Participants = [new Participant { Name = request.Owner.Name }]
        };
        await roomRepository.AddAsync(room);
        return new CreateRoomResponse(room.Id.ToString());
    }
}