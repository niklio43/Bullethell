using UnityEngine;

public abstract class Weapon : ScriptableObject
{
    Sprite _sprite;
    int _damage;
    string _itemName;

    public Sprite Sprite {get { return _sprite; } set { _sprite = value; }}
    public int Damage { get { return _damage; } set { _damage = value; } }
    public string ItemName { get { return _itemName; } set { _itemName = value; } }
}