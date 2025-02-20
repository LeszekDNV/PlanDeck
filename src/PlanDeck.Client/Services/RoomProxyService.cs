using Grpc.Net.Client;
using PlanDeck.Contracts.Room.Create;
using PlanDeck.Contracts.Room;
using ProtoBuf.Grpc.Client;
using MudBlazor;
using PlanDeck.Contracts.Room.Get;
using PlanDeck.Contracts.Room.Update;
using PlanDeck.Contracts.Dtos;

namespace PlanDeck.Client.Services;

public class RoomProxyService(GrpcChannel channel, ISnackbar snackbar, IUserLocalStorageService userService)
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

    public async Task ChangeSettings(string id, RoomSettingsDto settings)
    {
        UpdateRoomRequest request = new()
        {
            Id = id,
            RoomSettings = settings,
            User = await userService.GetUserAsync()
        };

        try
        {
            UpdateRoomResponse response = await _roomService.UpdateRoom(request);
            if (response.Success)
            {
                snackbar.Add("Room settings updated", Severity.Success);
            }
            else
            {
                snackbar.Add("Failed to update room settings", Severity.Error);
            }
        }
        catch (Exception ex)
        {
            snackbar.Add(ex.Message, Severity.Error);
        }
    }

    public async Task<RoomSettingsDto?> GetRoomSettings(string? id)
    {
        if (string.IsNullOrEmpty(id))
        {
            snackbar.Add("Room settings not found.", Severity.Error);
            return null;
        }

        GetRoomSettingsRequest request = new(id);
        try
        {
            GetRoomSettingsResponse response = await _roomService.GetRoomSettings(request);
            if (response?.RoomSettings != null)
            {
                return response.RoomSettings;
            }

            snackbar.Add("Room settings not found.", Severity.Error);
            return null;
        }
        catch (Exception ex)
        {
            snackbar.Add(ex.Message, Severity.Error);
        }
        return null;
    }
}
