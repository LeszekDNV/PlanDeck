using PlanDeck.Contracts.Dtos;
using PlanDeck.Contracts.Room.Create;

namespace PlanDeck.Application.Interfaces;

public interface IRoomService
{
    Task<RoomDto> CreateRoomAsync(CreateRoomRequest request);
}
