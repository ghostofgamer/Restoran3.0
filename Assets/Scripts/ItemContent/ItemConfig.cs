using Enums;
using UnityEngine;
using WalletContent;

[System.Serializable]
public class ItemConfig
{
    public ItemType ItemType;
    public ItemType Category;
    public DollarValue Price;
    public string ItemName;
    public Sprite Sprite;
    public Sprite SpriteNotBackground;
}