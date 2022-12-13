using UnityEngine;

public enum Rarity
{
    Common,
    Rare,
    Epic,
    Legendary
}

public abstract class Item : ScriptableObject
{
    public int Id;
    Sprite _sprite;
    string _itemName;
    Rarity _rarity;

    public Sprite Sprite { get { return _sprite; } set { _sprite = value; } }
    public string ItemName { get { return _itemName; } set { _itemName = value; } }
    public Rarity Rarity { get { return _rarity; } set { _rarity = value; } }
}

[System.Serializable]
public class ItemObject
{
    public string Name;
    public int Id;
    public ItemObject(Item item)
    {
        Name = item.name;
        Id = item.Id;
    }
}