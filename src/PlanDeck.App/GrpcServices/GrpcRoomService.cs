using PlanDeck.Contracts.Room;
using PlanDeck.Contracts.Room.Create;
using ProtoBuf.Grpc;

namespace PlanDeck.App.GrpcServices;

public class GrpcRoomService : IGrpcRoomService
{
    public async Task<CreateRoomResponse> CreateRoom(CreateRoomRequest request, CallContext context = default)
    {
        return new CreateRoomResponse(Guid.NewGuid().ToString());
    }
}
