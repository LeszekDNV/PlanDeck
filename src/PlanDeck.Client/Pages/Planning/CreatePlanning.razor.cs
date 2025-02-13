using Microsoft.AspNetCore.Components.Forms;
using PlanDeck.Contracts.Room.Create;

namespace PlanDeck.Client.Pages.Planning;

public partial class CreatePlanning
{
    bool success;
    private CreateRoomRequest model = new CreateRoomRequest();

    private void OnValidSubmit(EditContext context)
    {
        success = true;
        StateHasChanged();
    }
}
