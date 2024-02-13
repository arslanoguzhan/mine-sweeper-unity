struct BoardCreatedSignal
{
    public Level Level {get; private set;}

    public BoardCreatedSignal(Level level)
    {
        Level = level;
    }
}