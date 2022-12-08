using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public CharacterStats characterStats;

    void Awake()
    {
        characterStats = Instantiate(characterStats);
        Init();
    }

    protected virtual void Init() { }

    public virtual void TakeDamage(float value)
    {
        characterStats.Health -= value;
        if (characterStats.Health <= 0) { OnDeath(); }
    }

    public virtual void Heal(float value)
    {
        if (characterStats.Health + value > characterStats.MaxHealth) { characterStats.Health = characterStats.MaxHealth; return; }
        characterStats.Health += value;
    }

    protected virtual void OnDeath()
    {
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        Destroy(gameObject, 0.4f);
    }
}