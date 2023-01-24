using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BulletHell.Stats;

public class EnemyWeapon : MonoBehaviour
{
    Animator _animator;
    BoxCollider2D _boxCollider;
    bool attacking = false;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _boxCollider = GetComponent<BoxCollider2D>();
        _boxCollider.enabled = false;
    }

    public void Attack(DamageInfo damage)
    {
        _animator.Play("Attack");
        _animator.Update(Time.deltaTime);

        //_animator.GetCurrentAnimatorClipInfo(0)[0].clip.length;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!attacking) { return; }

        if (collision.CompareTag("Player")) {
            Debug.Log("Owie!!!! -Niklas");
            //DEAL DAMAGE
        }
    }
}
