using System.Collections.ObjectModel;
using Microsoft.AspNetCore.Components;
using PlanDeck.Contracts.Dtos;

namespace PlanDeck.Client.Pages.Planning;

public partial class PlanningRoom
{
    private ObservableCollection<UserDto> users = [];

    [Parameter] 
    public string? Id { get; set; }

    protected async override Task OnInitializedAsync()
    {

    }
}
