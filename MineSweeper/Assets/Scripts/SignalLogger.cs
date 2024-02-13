using UnityEngine;
using Zenject;

class SignalLogger
{
    private readonly SignalBus _signalBus;

    public SignalLogger(SignalBus signalBus)
    {
        _signalBus = signalBus;

        SubscribeEvents();
    }

    private void SubscribeEvents()
    {
        _signalBus.Subscribe<BoardCreatedSignal>(Log);

        _signalBus.Subscribe<SafeBoxOpenedSignal>(Log);
        _signalBus.Subscribe<SafeBoxFlaggedSignal>(Log);
        _signalBus.Subscribe<SafeBoxUnFlaggedSignal>(Log);

        _signalBus.Subscribe<BombBoxOpenedSignal>(Log);
        _signalBus.Subscribe<BombBoxFlaggedSignal>(Log);
        _signalBus.Subscribe<BombBoxUnFlaggedSignal>(Log);

        _signalBus.Subscribe<VictorySignal>(Log);
        _signalBus.Subscribe<DefeatSignal>(Log);
    }

    private void UnsubscribeEvents()
    {
        _signalBus.Unsubscribe<BoardCreatedSignal>(Log);

        _signalBus.Unsubscribe<SafeBoxOpenedSignal>(Log);
        _signalBus.Unsubscribe<SafeBoxFlaggedSignal>(Log);
        _signalBus.Unsubscribe<SafeBoxUnFlaggedSignal>(Log);

        _signalBus.Unsubscribe<BombBoxOpenedSignal>(Log);
        _signalBus.Unsubscribe<BombBoxFlaggedSignal>(Log);
        _signalBus.Unsubscribe<BombBoxUnFlaggedSignal>(Log);

        _signalBus.Subscribe<VictorySignal>(Log);
        _signalBus.Subscribe<DefeatSignal>(Log);
    }

    private void Log<T>(T signal)
    {
        //Debug.Log($"signal: {signal.GetType().Name}");
    }
}