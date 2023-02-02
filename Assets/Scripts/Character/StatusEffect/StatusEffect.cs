using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BulletHell.Stats;
using UnityEngine.VFX;

public class StatusEffect : MonoBehaviour
{
    public Sprite Icon;
    public VisualEffect Vfx;
    public float TickSpeed = 1;
    public float Lifetime;
    public bool Stackable;

    protected float _nextTickTime = 0f;
    protected float timer = 0f;
    protected Character _character;
    protected SpriteRenderer _sr;
    protected Transform _transform;
    protected Stats _stats;
    public void Initialize(Character owner)
    {
        _character = owner;
        _sr = _character.GetComponent<SpriteRenderer>();
        _transform = owner.transform;
        _stats = owner.Stats;
    }

    public void ResetAbility()
    {
        timer = 0f;
        _nextTickTime = 0f;
    }
}
