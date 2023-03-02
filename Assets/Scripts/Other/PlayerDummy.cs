using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BulletHell.EffectInterfaces;
using BulletHell;

public class PlayerDummy : MonoBehaviour, IDamageable
{
    [SerializeField] float Health = 100;

    public void Damage(DamageValue Damage)
    {
        Health -= Damage.GetDamage();
    }
}
