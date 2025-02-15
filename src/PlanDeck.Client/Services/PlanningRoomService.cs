using PlanDeck.Contracts.Room;
using PlanDeck.Contracts.Room.Create;

namespace PlanDeck.Client.Services;

public class PlanningRoomService(RoomProxyService roomProxyService) : IPlanningRoomService
{
    public string? ActivePlanningId { get; private set; }
    public RoomSettingsDto? ActivePlanningSettings { get; private set; }
    public bool IsPlanningActive { get; private set; }

    public async Task<RoomSettingsDto?> GetRoomSettings(string? id)
    {
        RoomSettingsDto? result = await roomProxyService.GetRoomSettings(id);
        return result;
    }

    public void StartPlanning(RoomSettingsDto? settings)
    {
        IsPlanningActive = true;
        ActivePlanningSettings = settings;
        PlanningRoomChanged?.Invoke(this, settings);
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

    public event EventHandler<RoomSettingsDto?>? PlanningRoomChanged;
}

public interface IPlanningRoomService
{
    bool IsPlanningActive { get; }
    RoomSettingsDto? ActivePlanningSettings { get; }

    Task ChangeSettings(RoomSettingsDto settings);
    Task<CreateRoomResponse> CreateRoom(CreateRoomRequest roomRequest);
    void EndPlanning();
    Task<RoomSettingsDto?> GetRoomSettings(string? id);
    void StartPlanning(RoomSettingsDto? settings);

    event EventHandler<RoomSettingsDto?> PlanningRoomChanged;

}
