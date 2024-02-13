using UnityEngine;

class AssetManager : IAssetManager
{
    private static readonly Sprite[] sprites = new Sprite[12];

    public void LoadAssets()
    {
        sprites[SpriteIndex.Zero.ToInt()] = Resources.Load<Sprite>("tile-0");
        sprites[SpriteIndex.One.ToInt()] = Resources.Load<Sprite>("tile-1");
        sprites[SpriteIndex.Two.ToInt()] = Resources.Load<Sprite>("tile-2");
        sprites[SpriteIndex.Three.ToInt()] = Resources.Load<Sprite>("tile-3");
        sprites[SpriteIndex.Four.ToInt()] = Resources.Load<Sprite>("tile-4");
        sprites[SpriteIndex.Five.ToInt()] = Resources.Load<Sprite>("tile-5");
        sprites[SpriteIndex.Six.ToInt()] = Resources.Load<Sprite>("tile-6");
        sprites[SpriteIndex.Seven.ToInt()] = Resources.Load<Sprite>("tile-7");
        sprites[SpriteIndex.Eight.ToInt()] = Resources.Load<Sprite>("tile-8");
        sprites[SpriteIndex.Bomb.ToInt()] = Resources.Load<Sprite>("tile-bomb");
        sprites[SpriteIndex.Button.ToInt()] = Resources.Load<Sprite>("tile-button");
        sprites[SpriteIndex.Flag.ToInt()] = Resources.Load<Sprite>("tile-flag");
    }

    public Sprite GetSprite(int index)
    {
        return sprites[index];
    }

    public Sprite GetSprite(SpriteIndex spriteIndex)
    {
        return sprites[spriteIndex.ToInt()];
    }
}