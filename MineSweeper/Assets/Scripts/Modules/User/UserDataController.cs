using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;

class UserDataController : IUserDataController
{
    private readonly IFileManager _fileManager;

    private readonly IJsonSerializer _jsonSerializer;

    private string UserFileName => $"user.json";

    private User _user;

    public UserDataController(
        IFileManager fileManager,
        IJsonSerializer jsonSerializer
    ){
        _fileManager = fileManager;
        _jsonSerializer = jsonSerializer;
    }

    public string GetUserGuid()
    {
        return _user.UserGuid;
    }

    public Level GetPreviousLevel()
    {
        return _user.PreviousLevel;
    }

    public async Task LoadUserAsync()
    {
        if (!await _fileManager.FileExistsAsync(UserFileName))
        {
            var newUser = new User()
            {
                UserGuid = Guid.NewGuid().ToString(),
                PreviousLevel = new Level()
                {
                    Number = 0,
                    RowCount = 0,
                    ColCount = 0,
                    BombCount = 0
                },
            };

            await SaveUserAsync(newUser);
        }
        else
        {
            var userJson = await _fileManager.FileReadAsync(UserFileName);
            _user = _jsonSerializer.Deserialize<User>(userJson);
        }
    }

    public async Task SaveUserAsync(User user)
    {
        await Task.Yield();

        _user = user;
        var userJson = _jsonSerializer.Serialize(_user);
        await _fileManager.FileWriteAsync(UserFileName, userJson);
    }
}
