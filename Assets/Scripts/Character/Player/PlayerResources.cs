using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BulletHell.EffectInterfaces;
using BulletHell;

public class PlayerResources : MonoBehaviour, IDamageable
{
    public float Health;
    public float Stamina;
    public float MaxStamina;


    void Update()
    {
        if(Stamina >= MaxStamina) { return; }
        Stamina += Time.deltaTime;
    }

    public void Damage(DamageValue Damage)
    {
        Health -= Damage.GetDamage();
    }
}
