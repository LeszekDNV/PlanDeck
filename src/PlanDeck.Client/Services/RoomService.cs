using Grpc.Net.Client;
using PlanDeck.Contracts.Room;
using PlanDeck.Contracts.Room.Create;
using ProtoBuf.Grpc.Client;

namespace PlanDeck.Client.Services;

public class RoomService
{
    private readonly IGrpcRoomService _roomService;
    public RoomService(GrpcChannel channel) => _roomService = channel.CreateGrpcService<IGrpcRoomService>();

    public async Task<CreateRoomResponse> CreateRoom(CreateRoomRequest request) => await _roomService.CreateRoom(request);
}
