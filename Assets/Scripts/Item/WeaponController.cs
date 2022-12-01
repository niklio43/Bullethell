using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BulletHell.Emitters;
public class WeaponController : MonoBehaviour
{
    [SerializeField] Weapon _weapon;

    void Awake()
    {
        GetComponent<SpriteRenderer>().sprite = _weapon.Sprite;
        GetComponent<Emitter>().data = _weapon.EmitterData;
    }

    void Update()
    {
        
    }
}
