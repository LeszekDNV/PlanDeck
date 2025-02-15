using System.Collections.ObjectModel;
using Microsoft.AspNetCore.Components;
using PlanDeck.Client.Services;
using PlanDeck.Contracts.Dtos;
using PlanDeck.Contracts.Room;

namespace PlanDeck.Client.Pages.Planning;

public partial class PlanningRoom
{
    private ObservableCollection<UserDto> users = [];
    private RoomSettingsDto? roomSettings = new();
    private bool IsLoaded { get; set; }

    [Parameter] 
    public string? Id { get; set; }

    [Inject] 
    private IPlanningRoomService PlanningRoomService { get; set; } = null!;

    protected override async Task OnInitializedAsync()
    {
        PlanningRoomService.PlanningRoomChanged += OnPlanningRoomChanged;
    }
    
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            roomSettings = await GetRoomSettings();
            PlanningRoomService.StartPlanning(roomSettings);
            IsLoaded = true;
            StateHasChanged();
        }
    }

    private void OnPlanningRoomChanged(object? sender, RoomSettingsDto? settings)
    {
        roomSettings = settings;
        StateHasChanged();
    }

    private async Task<RoomSettingsDto?> GetRoomSettings()
    {
        var settings = await PlanningRoomService.GetRoomSettings(Id);
        return settings;
    }
}
