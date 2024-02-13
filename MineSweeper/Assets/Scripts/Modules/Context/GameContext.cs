using System;
using Zenject;

class GameContext : IDisposable
{
    private readonly SignalBus _signalBus;

    public int BombBoxCount {get; private set;}

    public int SafeBoxCount {get; private set;}

    public int FlagsMarked {get; private set;} = 0;

    public int FlagsCorrect {get; private set;} = 0;

    public int OpenedCount {get; private set;} = 0;

    private Level _level;

    public GameStatus GameStatus {get; private set;} = GameStatus.Loading;

    public GameContext(
        SignalBus signalBus
    ){
        _signalBus = signalBus;

        SubscribeEvents();
    }

    public void Dispose()
    {
        UnsubscribeEvents();
    }

    private void SubscribeEvents()
    {
        _signalBus.Subscribe<BoardCreatedSignal>(OnBoardCreated);

        _signalBus.Subscribe<SafeBoxOpenedSignal>(OnSafeBoxOpened);
        _signalBus.Subscribe<SafeBoxFlaggedSignal>(OnSafeBoxFlagged);
        _signalBus.Subscribe<SafeBoxUnFlaggedSignal>(OnSafeBoxUnFlagged);

        _signalBus.Subscribe<BombBoxOpenedSignal>(OnBombBoxOpened);
        _signalBus.Subscribe<BombBoxFlaggedSignal>(OnBombBoxFlagged);
        _signalBus.Subscribe<BombBoxUnFlaggedSignal>(OnBombBoxUnFlagged);
    }

    private void UnsubscribeEvents()
    {
        _signalBus.Unsubscribe<BoardCreatedSignal>(OnBoardCreated);

        _signalBus.Unsubscribe<SafeBoxOpenedSignal>(OnSafeBoxOpened);
        _signalBus.Unsubscribe<SafeBoxFlaggedSignal>(OnSafeBoxFlagged);
        _signalBus.Unsubscribe<SafeBoxUnFlaggedSignal>(OnSafeBoxUnFlagged);

        _signalBus.Unsubscribe<BombBoxOpenedSignal>(OnBombBoxOpened);
        _signalBus.Unsubscribe<BombBoxFlaggedSignal>(OnBombBoxFlagged);
        _signalBus.Unsubscribe<BombBoxUnFlaggedSignal>(OnBombBoxUnFlagged);
    }

    private void OnBoardCreated(BoardCreatedSignal boardCreatedSignal)
    {
        _level = boardCreatedSignal.Level;

        BombBoxCount = _level.BombCount;
        SafeBoxCount = (_level.RowCount * _level.ColCount) - BombBoxCount;

        GameStatus = GameStatus.Playing;
    }

    private void OnSafeBoxOpened()
    {
        OpenedCount++;

        CheckVictory();
    }

    private void OnSafeBoxFlagged()
    {
        FlagsMarked++;
    }

    private void OnSafeBoxUnFlagged()
    {
        FlagsMarked--;
    }

    private void OnBombBoxOpened()
    {
        OpenedCount++;

        GameStatus = GameStatus.Defeat;
        _signalBus.Fire(new DefeatSignal());
    }

    private void OnBombBoxFlagged()
    {
        FlagsMarked++;
        FlagsCorrect++;

        CheckVictory();
    }

    private void OnBombBoxUnFlagged()
    {
        FlagsMarked--;
        FlagsCorrect--;
    }

    private void CheckVictory()
    {
        if (FlagsCorrect == BombBoxCount && OpenedCount == SafeBoxCount)
        {
            GameStatus = GameStatus.Victory;
            _signalBus.Fire(new VictorySignal());
        }
    }
}
