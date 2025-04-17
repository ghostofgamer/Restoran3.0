using Enums;
using UnityEngine;
using WalletContent;

[System.Serializable]
public class ItemConfig 
{
    public ItemType itemType;
    public ItemType category;
    public DollarValue price;
    public string itemName;
    public Sprite _sprite;
}