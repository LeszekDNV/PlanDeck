using PlanDeck.Application.Interfaces;
using PlanDeck.Contracts.Room;
using PlanDeck.Contracts.Room.Create;
using PlanDeck.Contracts.Room.Get;
using PlanDeck.Contracts.Room.State;
using PlanDeck.Contracts.Room.Update;
using ProtoBuf.Grpc;

namespace PlanDeck.App.GrpcServices;

public class GrpcRoomService(IRoomService roomService) : IGrpcRoomService
{
    public async Task<CreateRoomResponse> CreateRoom(CreateRoomRequest request, CallContext context = default)
    {
        Guid result = await roomService.CreateRoomAsync(request);
        return new CreateRoomResponse(result.ToString());
    }

    public Task<UpdateRoomResponse> UpdateRoom(UpdateRoomRequest request, CallContext context = default) 
        => roomService.UpdateRoomAsync(request);

    public Task<GetRoomSettingsResponse> GetRoomSettings(GetRoomSettingsRequest request, CallContext context = default)
        => roomService.GetRoomSettings(request);

    public IAsyncEnumerable<ServerStreamMessage> Connect(SubscribeRequest request, CallContext context = default) 
        => roomService.Connect(request, context.CancellationToken);
}