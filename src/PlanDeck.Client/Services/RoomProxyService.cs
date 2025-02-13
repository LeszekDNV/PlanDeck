using Grpc.Net.Client;
using PlanDeck.Contracts.Room.Create;
using PlanDeck.Contracts.Room;
using ProtoBuf.Grpc.Client;
using MudBlazor;

namespace PlanDeck.Client.Services;

public class RoomProxyService(GrpcChannel channel, ISnackbar snackbar)
{
    private readonly IGrpcRoomService _roomService = channel.CreateGrpcService<IGrpcRoomService>();

    public async Task<CreateRoomResponse> CreateRoom(CreateRoomRequest request)
    {
		try
		{
			CreateRoomResponse result = await _roomService.CreateRoom(request);
			return result;
		}
		catch (Exception ex)
		{
			snackbar.Add(ex.Message, Severity.Error);
            return new();
        }
    }
}
