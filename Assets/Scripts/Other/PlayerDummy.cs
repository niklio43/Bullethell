using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BulletHell.EffectInterfaces;
using BulletHell;

public class PlayerDummy : MonoBehaviour, IDamageable
{
    #region Private Fields
    [SerializeField] float Health = 100;
    #endregion

    #region Public Methods
    public void Damage(DamageValue Damage)
    {
        Health -= Damage.GetDamage();
    }
    #endregion
}
