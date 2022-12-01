using UnityEngine;

public abstract class Weapon : Item
{
    int _damage;

    public int Damage { get { return _damage; } set { _damage = value; } }
}