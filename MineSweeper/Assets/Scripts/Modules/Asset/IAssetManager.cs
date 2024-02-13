using UnityEngine;

interface IAssetManager
{
    void LoadAssets();
    Sprite GetSprite(int index);
    Sprite GetSprite(SpriteIndex spriteIndex);
}