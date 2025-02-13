using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using PlanDeck.Client.Services;
using PlanDeck.Client.Shared.Dialgogs;
using PlanDeck.Contracts.Dtos;
using PlanDeck.Contracts.Room.Create;

namespace PlanDeck.Client.Pages.Planning;

public partial class CreatePlanning
{
    private readonly CreateRoomRequest model = new();
    private readonly DialogOptions dialogOptions = new() { BackdropClick = false };

    [Inject] public IDialogService DialogService { get; set; } = null!;

    [Inject] public NavigationManager NavigationManager { get; set; } = null!;

    [Inject] public RoomProxyService RoomService { get; set; } = null!;

    [Inject] public IUserLocalStorageService UserService { get; set; } = null!;

    private async Task OnValidSubmit(EditContext context)
    {
        UserDto user = await UserService.GetUserAsync() ?? new UserDto
        {
            Name = await ShowCreateUserModal()
        };

        model.Owner = user;
        model.Name = string.IsNullOrWhiteSpace(model.Name) ? "My Planning Room" : model.Name;
        CreateRoomResponse response = await RoomService.CreateRoom(model);
        user.LastPlanningRoom = response.Id;
        await UserService.SaveUserAsync(user);

        NavigationManager.NavigateTo($"/planning/{response.Id}");
    }

    private async Task<string> ShowCreateUserModal()
    {
        IDialogReference dialog = await DialogService.ShowAsync<CreateUserDialog>("", dialogOptions);
        DialogResult? result = await dialog.Result;
        return (result?.Data as string)!;
    }
}