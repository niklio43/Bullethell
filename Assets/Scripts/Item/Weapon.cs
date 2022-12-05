using BulletHell.Emitters;
public abstract class Weapon : Item
{
    int _damage;
    EmitterData _emitterData;

    public int Damage { get { return _damage; } set { _damage = value; } }
    public EmitterData EmitterData { get { return _emitterData; } set { _emitterData = value; } }
}