namespace PlanDeck.Client.Services;

using Blazored.LocalStorage;
using PlanDeck.Contracts.Dtos;
using System.Threading.Tasks;

public interface IUserLocalStorageService
{
    Task<UserDto?> GetUserAsync();
    Task SaveUserAsync(UserDto user);
    Task UpdateUserAsync(UserDto user);
    Task RemoveUserAsync();

    event EventHandler<UserDto> UserLoggedIn;
    event EventHandler<UserDto> UserChanged;
    event EventHandler UserLoggedOut;
}

public class UserLocalStorageService(ILocalStorageService localStorageService)
    : IUserLocalStorageService
{
    private const string UserKey = "PD_CurrentUser";

    /// <summary>
    /// Odczyt użytkownika z local storage
    /// </summary>
    /// <returns></returns>
    public async Task<UserDto?> GetUserAsync()
    {
        return await localStorageService.GetItemAsync<UserDto>(UserKey);
    }

    /// <summary>
    /// Zapis/aktualizacja (nadpisanie) użytkownika w local storage
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    public async Task SaveUserAsync(UserDto user)
    {
        await localStorageService.SetItemAsync(UserKey, user);
        UserLoggedIn?.Invoke(this, user);
    }

    /// <summary>
    /// Aktualizacja wybranych pól użytkownika – jeśli chcesz zaktualizować tylko niektóre dane
    /// (np. uwzględnić stan obecnie zapisanego obiektu)
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    public async Task UpdateUserAsync(UserDto user)
    {
        var existingUser = await GetUserAsync();

        if (existingUser is not null)
        {
            // Możesz oczywiście rozbudować tę logikę
            existingUser.Name = user.Name ?? existingUser.Name;
            existingUser.LastPlanningRoom = user.LastPlanningRoom ?? existingUser.LastPlanningRoom;

            await SaveUserAsync(existingUser);
            UserChanged?.Invoke(this, user);
        }
        else
        {
            // Jeśli w local storage nie ma jeszcze danych – zapisz je jako nowe
            await SaveUserAsync(user);
        }
    }

    /// <summary>
    /// Usunięcie użytkownika z local storage
    /// </summary>
    /// <returns></returns>
    public async Task RemoveUserAsync()
    {
        await localStorageService.RemoveItemAsync(UserKey);
        UserLoggedOut?.Invoke(this, EventArgs.Empty);
    }

    public event EventHandler<UserDto>? UserLoggedIn;
    public event EventHandler<UserDto>? UserChanged;
    public event EventHandler? UserLoggedOut;
}
