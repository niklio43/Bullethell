using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BulletHell.Stats;

public class EnemyWeapon : MonoBehaviour
{
    [SerializeField] Vector2 _offset;
    [SerializeField] float _radius = 1;


    private void Awake()
    {
    }

    public void Attack(DamageInfo damage)
    {

        //_animator.GetCurrentAnimatorClipInfo(0)[0].clip.length;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.black;

        Gizmos.DrawWireSphere(transform.parent.position + transform.parent.right, _radius);
    }
}
