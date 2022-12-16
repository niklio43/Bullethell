using UnityEngine;

public enum Rarity
{
    Common,
    Rare,
    Epic,
    Legendary
}

public enum ItemType
{
    Weapon,
    Headgear,
    Chestgear,
    Legwear,
    Consumable
}

public abstract class Item : ScriptableObject
{
    Sprite _sprite;
    public bool Stackable;
    string _itemName;
    Rarity _rarity;
    ItemType _itemType;
    public ItemObject data = new ItemObject();

    public Sprite Sprite { get { return _sprite; } set { _sprite = value; } }
    public string ItemName { get { return _itemName; } set { _itemName = value; } }
    public Rarity Rarity { get { return _rarity; } set { _rarity = value; } }
    public ItemType ItemType { get { return _itemType; } set { _itemType = value; } }
}

[System.Serializable]
public class ItemObject
{
    public string Name;
    public int Id = -1;
    public ItemObject()
    {
        Name = "";
        Id = -1;
    }
    public ItemObject(Item item)
    {
        Name = item.name;
        Id = item.data.Id;
    }
}