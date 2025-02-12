using PlanDeck.Contracts.Room.Create;
using ProtoBuf.Grpc;
using ProtoBuf.Grpc.Configuration;

namespace PlanDeck.Contracts.Room;

[Service]
public interface IGrpcRoomService
{
    Task<CreateRoomResponse> CreateRoom(CreateRoomRequest request, CallContext context = default);
}
