using Microsoft.AspNetCore.Components;
using PlanDeck.Client.Services;
using PlanDeck.Contracts.Dtos;

namespace PlanDeck.Client.Shared.Components;

public partial class UserMenu
{
    private string userFirstLetter = "";
    private UserDto? activeUser = null;

    [Inject]
#pragma warning disable CS8618 
    public IUserLocalStorageService UserLocalStorageService { get; set; }
#pragma warning restore CS8618 

    protected override Task OnInitializedAsync()
    {
        UserLocalStorageService.UserChanged += OnUserChanged;
        UserLocalStorageService.UserLoggedIn += OnUserChanged;

        return Task.CompletedTask;
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            activeUser = await UserLocalStorageService.GetUserAsync();
            if (activeUser  != null)
            {
                userFirstLetter = activeUser.Name[0].ToString().ToUpper();
                StateHasChanged();
            }
        }
    }

    private async Task Logout()
    {
        await UserLocalStorageService.RemoveUserAsync();
        activeUser = null;
        StateHasChanged();
    }

    private void OnUserChanged(object? sender, UserDto user)
    {
        activeUser = user;
        StateHasChanged();
    }
}
