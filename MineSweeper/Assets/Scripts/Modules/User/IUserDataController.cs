using System;
using System.Threading.Tasks;

interface IUserDataController
{
    string GetUserGuid();

    Level GetPreviousLevel();

    Task LoadUserAsync();

    Task SaveUserAsync(User user);
}