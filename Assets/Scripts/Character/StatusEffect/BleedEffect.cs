using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Status Effect/BleedEffect")]
public class BleedEffect : StatusEffectData
{
    [SerializeField] float _DOT;

    public override void Perform()
    {
        Debug.Log("Applied Effect: " + Name);
        timer = 0;
        _nextTickTime = 0;
    }

    public override void UpdateStatus(float dt)
    {
        timer += dt;
        if (timer >= Lifetime) { timer = 0; _nextTickTime = 0; _character.RemoveEffect(); }

        if(timer > _nextTickTime)
        {
            _nextTickTime += TickSpeed;
            _stats["Hp"].Value -= _DOT;
            Debug.Log("HP: " + _stats["Hp"].Value);
        }
    }
}
