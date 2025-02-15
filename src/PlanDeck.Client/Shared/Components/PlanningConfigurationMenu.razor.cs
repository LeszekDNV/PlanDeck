using Microsoft.AspNetCore.Components;
using MudBlazor;
using PlanDeck.Client.Services;
using PlanDeck.Client.Shared.Dialgogs;
using PlanDeck.Contracts.Room;

namespace PlanDeck.Client.Shared.Components;

public partial class PlanningConfigurationMenu
{
    private RoomSettingsDto? activePlanningSettings;
    private DialogOptions dialogOptions = new() { CloseButton = true };
    private bool isPlanningActive;

    [Inject] public IDialogService DialogService { get; set; } = null!;
    [Inject] public IPlanningRoomService PlanningRoomService { get; set; } = null!;

    protected override Task OnInitializedAsync()
    {
        PlanningRoomService.PlanningRoomChanged += OnPlanningRoomChanged;
        return base.OnInitializedAsync();
    }

    private void OnPlanningRoomChanged(object? sender, RoomSettingsDto? planningSettings)
    {
        isPlanningActive = PlanningRoomService.IsPlanningActive;
        activePlanningSettings = planningSettings;
        StateHasChanged();
    }

    private async Task OpenRoomSettingsDialog()
    {
        DialogParameters<ChangeRoomSettingsDialog> parameters = new()
        {
            { x => x.Model, activePlanningSettings }
        };
        IDialogReference dialog = await DialogService.ShowAsync<ChangeRoomSettingsDialog>("", parameters, dialogOptions);
        DialogResult? result = await dialog.Result;

        if (result?.Data is not RoomSettingsDto roomSettings || result.Canceled) return;
        await PlanningRoomService.ChangeSettings(roomSettings);
    }
}