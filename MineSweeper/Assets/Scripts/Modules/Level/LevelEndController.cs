using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Zenject;

class LevelEndController : IDisposable
{
    private readonly SignalBus _signalBus;
    private readonly ISwitcher _switcher;
    private readonly IUserDataController _userDataController;
    private readonly ILevelDataController _levelDataController;

    public LevelEndController(
        SignalBus signalBus,
        ISwitcher switcher,
        IUserDataController userDataController,
        ILevelDataController levelDataController
    ){
        _signalBus = signalBus;
        _switcher = switcher;
        _userDataController = userDataController;
        _levelDataController = levelDataController;

        SubscribeSignals();
    }

    private void SubscribeSignals()
    {
        _signalBus.Subscribe<VictorySignal>(OnVictory);
        _signalBus.Subscribe<DefeatSignal>(OnDefeat);
    }

    private void UnsubscribeSignals()
    {
        _signalBus.Unsubscribe<VictorySignal>(OnVictory);
        _signalBus.Unsubscribe<DefeatSignal>(OnDefeat);
    }

    private void OnVictory()
    {
        OnVictoryAsync().Forget();
    }

    private async UniTaskVoid OnVictoryAsync()
    {
        _switcher.DisableWorld();

        var user = new User()
        {
            UserGuid = _userDataController.GetUserGuid(),
            PreviousLevel = _levelDataController.GetCurrentLevel()
        };

        await _userDataController.SaveUserAsync(user);        
    }

    private void OnDefeat()
    {
        _switcher.DisableWorld();
    }

    public void Dispose()
    {
        UnsubscribeSignals();
    }
}