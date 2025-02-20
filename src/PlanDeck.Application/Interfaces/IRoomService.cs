using PlanDeck.Contracts.Dtos;
using PlanDeck.Contracts.Room.Create;
using PlanDeck.Contracts.Room.Get;
using PlanDeck.Contracts.Room.State;
using PlanDeck.Contracts.Room.Update;

namespace PlanDeck.Application.Interfaces;

public interface IRoomService
{
    Task<Guid> CreateRoomAsync(CreateRoomRequest request);
    Task<UpdateRoomResponse> UpdateRoomAsync(UpdateRoomRequest request);
    Task<GetRoomSettingsResponse> GetRoomSettings(GetRoomSettingsRequest request);
    IAsyncEnumerable<ServerStreamMessage> Connect(SubscribeRequest request, CancellationToken cancellationToken);
}
