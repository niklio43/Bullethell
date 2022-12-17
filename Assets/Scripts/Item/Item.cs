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

public enum Attributes
{
    Attack,
    Defense,
    Stamina
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

    public ItemObject CreateItem()
    {
        ItemObject newItem = new ItemObject(this);
        return newItem;
    }
}

[System.Serializable]
public class ItemObject
{
    public string Name;
    public int Id = -1;
    public ItemBuff[] buffs;

    public ItemObject()
    {
        Name = "";
        Id = -1;
    }
    public ItemObject(Item item)
    {
        Name = item.name;
        Id = item.data.Id;
        buffs = new ItemBuff[item.data.buffs.Length];
        for (int i = 0; i < buffs.Length; i++)
        {
            buffs[i] = new ItemBuff(item.data.buffs[i].min, item.data.buffs[i].max)
            {
                attribute = item.data.buffs[i].attribute
            };
        }
    }
}

[System.Serializable]
public class ItemBuff : IModifier
{
    public Attributes attribute;
    public int value;
    public int min;
    public int max;
    public ItemBuff(int _min, int _max)
    {
        min = _min;
        max = _max;
        GenerateValue();
    }

    public void AddValue(ref int baseValue)
    {
        baseValue += value;
    }

    public void GenerateValue()
    {
        value = UnityEngine.Random.Range(min, max);
    }
}