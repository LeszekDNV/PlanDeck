using PlanDeck.Contracts.Room.Create;
using PlanDeck.Contracts.Room.Get;
using PlanDeck.Contracts.Room.Update;
using ProtoBuf.Grpc;
using ProtoBuf.Grpc.Configuration;

namespace PlanDeck.Contracts.Room;

[Service]
public interface IGrpcRoomService
{
    Task<CreateRoomResponse> CreateRoom(CreateRoomRequest request, CallContext context = default);
    Task<UpdateRoomResponse> UpdateRoom(UpdateRoomRequest request, CallContext context = default);
    Task<GetRoomSettingsResponse> GetRoomSettings(GetRoomSettingsRequest request, CallContext context = default);
}
