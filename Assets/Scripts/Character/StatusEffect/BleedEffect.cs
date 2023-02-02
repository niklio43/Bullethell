using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BleedEffect : StatusEffect
{
    [SerializeField] float _DOT;

    public void Perform()
    {
        Debug.Log("Applied Effect: " + Name);
        timer = 0;
        _nextTickTime = 0;
    }

    public void UpdateStatus(float dt)
    {
        timer += dt;
        //if (timer >= Lifetime) { timer = 0; _nextTickTime = 0; _character.RemoveEffect(); }

        if (timer > _nextTickTime)
        {
            _nextTickTime += TickSpeed;
            _stats["Hp"].Value -= _DOT;
            Debug.Log("HP: " + _stats["Hp"].Value);
        }
    }
}
