using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [SerializeField] Weapon _weapon;
    void Awake()
    {
        GetComponent<SpriteRenderer>().sprite = _weapon.Sprite;
    }

    void Update()
    {
        
    }
}
