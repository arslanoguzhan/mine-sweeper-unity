enum SpriteIndex
{
    Zero = 0,
    One = 1,
    Two = 2,
    Three = 3,
    Four = 4,
    Five = 5,
    Six = 6,
    Seven = 7,
    Eight = 8,
    Bomb = 9,
    Button = 10,
    Flag = 11
}

static class SpriteIndexExtensions
{
    public static int ToInt(this SpriteIndex @this)
    {
        return (int)@this;
    }
}