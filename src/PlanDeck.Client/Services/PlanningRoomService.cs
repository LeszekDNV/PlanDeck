using MudBlazor;
using PlanDeck.Contracts.Dtos;
using PlanDeck.Contracts.Room.Create;
using PlanDeck.Contracts.Room.State;

namespace PlanDeck.Client.Services;

public class PlanningRoomService : IPlanningRoomService
{
    private readonly RoomProxyService roomProxyService;
    private readonly IConnectionProxyService connectionService;
    private readonly IUserLocalStorageService userService;
    private readonly ISnackbar snackbar;
    private UserDto? currentUser;

    public PlanningRoomService(
        RoomProxyService roomProxyService,
        IConnectionProxyService connectionService,
        IUserLocalStorageService userService,
        ISnackbar snackbar
    )
    {
        this.roomProxyService = roomProxyService;
        this.connectionService = connectionService;
        this.userService = userService;
        this.snackbar = snackbar;
        connectionService.OnServerMessage += HandleServerMessage;
    }    

    public string? ActivePlanningId { get; private set; }
    public RoomSettingsDto? ActivePlanningSettings { get; private set; }
    public bool IsPlanningActive { get; private set; }

    public async Task<RoomSettingsDto?> GetRoomSettings(string? id)
    {
        RoomSettingsDto? result = await roomProxyService.GetRoomSettings(id);
        return result;
    }

    public async Task ChangeSettings(RoomSettingsDto settings)
    {
        ActivePlanningSettings = settings;
        if (!string.IsNullOrEmpty(ActivePlanningId))
            await roomProxyService.ChangeSettings(ActivePlanningId, settings);
        PlanningRoomChanged?.Invoke(this, settings);
    }

    public async Task<CreateRoomResponse> CreateRoom(CreateRoomRequest roomRequest)
    {
        CreateRoomResponse result = await roomProxyService.CreateRoom(roomRequest);
        ActivePlanningId = result.Id;
        PlanningRoomChanged?.Invoke(this, new()
        {
            AutoRevealCards = roomRequest.AutoRevealCards,
            Name = roomRequest.Name,
            Owner = roomRequest.Owner,
            ShowAverage = roomRequest.ShowAverage,
            VotingSystem = roomRequest.VotingSystem,
            WhoCanManageIssues = roomRequest.WhoCanManageIssues,
            WhoCanRevealCards = roomRequest.WhoCanRevealCards
        });
        return result;
    }

    public void EndPlanning()
    {
        IsPlanningActive = false;
        ActivePlanningSettings = null;
        PlanningRoomChanged?.Invoke(this, null);
    }

    public async Task StartPlanning(RoomSettingsDto? settings)
    {
        currentUser = await userService.GetUserAsync();

        if (currentUser == null)
        {
            snackbar.Add("You cannot join to this room because user is not created.");
            return;
        }

        IsPlanningActive = true;
        ActivePlanningSettings = settings;
        connectionService.Connect(currentUser);
        PlanningRoomChanged?.Invoke(this, settings);
    }

    private void HandleServerMessage(ServerStreamMessage message)
    {
        snackbar.Add(message.EventType.ToString(), Severity.Info);
        switch (message.EventType)
        {
            case RoomEventTypes.PING:
                break;
            case RoomEventTypes.USER_JOINED:
                break;
            case RoomEventTypes.CARD_PLAYED:
                break;
            case RoomEventTypes.CARDS_REVEALED:
                break;
            case RoomEventTypes.SETTINGS_CHANGED:
                PlanningRoomChanged?.Invoke(this, message.RoomSettings);
                break;
            case RoomEventTypes.ACTIVE_ISSUE_CHANGED:
                break;
            case RoomEventTypes.USER_LEFT:
                break;
            default:
                break;
        }
    }

    public void Dispose()
    {
        _ = connectionService.DisposeAsync();
    }

    public event EventHandler<RoomSettingsDto?>? PlanningRoomChanged;
}

public interface IPlanningRoomService : IDisposable
{
    bool IsPlanningActive { get; }
    RoomSettingsDto? ActivePlanningSettings { get; }

    Task ChangeSettings(RoomSettingsDto settings);
    Task<CreateRoomResponse> CreateRoom(CreateRoomRequest roomRequest);
    void EndPlanning();
    Task<RoomSettingsDto?> GetRoomSettings(string? id);
    Task StartPlanning(RoomSettingsDto? settings);

    event EventHandler<RoomSettingsDto?> PlanningRoomChanged;
}
