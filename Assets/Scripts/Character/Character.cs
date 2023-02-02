using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BulletHell.Stats;
using BulletHell;
using BulletHell.StatusSystem;

public class Character : MonoBehaviour
{
    public Stats Stats;
    public StatusEffectData StatusEffect;
    protected SpriteRenderer _spriteRenderer;
    protected Animator _animator;

    public void Initialize()
    {
        Stats.TranslateListToDictionary();

        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _animator = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        Initialize();
    }

    private void Update()
    {
        StatusEffect.UpdateEffects(Time.deltaTime);
        OnUpdate();
    }

    protected virtual void OnUpdate() { }

    public virtual void TakeDamage(float damage)
    {
        Stats["Hp"].Value -= damage;

        if (Stats["Hp"].Value <= 0)
        {
            OnDeath();
        }

        if (_spriteRenderer == null) return;
        StartCoroutine(ResetMaterial());
    }


    IEnumerator ResetMaterial()
    {
        _spriteRenderer.material.SetInt("_hit", 1);
        yield return new WaitForSeconds(0.1f);
        _spriteRenderer.material.SetInt("_hit", 0);
    }

    public virtual void Heal(float amount)
    {
        Stats["Hp"].Value += amount;

        if (Stats["Hp"].Value > Stats["MaxHp"].Value)
        {
            Stats["Hp"].Value = Stats["MaxHp"].Value;
        }
    }

    public virtual void OnDeath()
    {
        Debug.Log("Dead");
        var stateID = Animator.StringToHash("OnDeath");
        if (_animator.HasState(0, stateID))
        {
            _animator.Play("OnDeath");
            Destroy(gameObject, _animator.GetCurrentAnimatorClipInfo(0)[0].clip.length);
            return;
        }
        Destroy(gameObject);
    }
}