using UnityEngine;

public abstract class Item : ScriptableObject
{
    Sprite _sprite;
    string _itemName;

    public Sprite Sprite { get { return _sprite; } set { _sprite = value; } }
    public string ItemName { get { return _itemName; } set { _itemName = value; } }
}