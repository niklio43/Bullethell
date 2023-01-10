using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BulletHell.Emitters;
using BulletHell.Abilities;

public class WeaponController : MonoBehaviour
{
    public Transform CircleOrigin;
    public float Radius;

    public void AssignWeapon(Weapon weapon)
    {
        weapon.Initialize(gameObject);
        GetComponent<Animator>().Play(weapon.WeaponIdleAnimation.name);
        GetComponent<SpriteRenderer>().sprite = weapon.Sprite;
        gameObject.name = weapon.name;

        foreach (Ability ability in weapon.AbilitySlot)
        {
            GetComponent<AbilityHolder>().abilities.Add(ability);
        }

        var playerSM = transform.GetComponentInParent<PlayerStateMachine>();
        playerSM.Weapon = weapon;
    }

    public void UnAssignWeapon(Weapon weapon)
    {
        weapon.UnInitialize(gameObject);
        GetComponent<SpriteRenderer>().sprite = null;

        GetComponent<AbilityHolder>().abilities.Clear();

        GetComponent<Animator>().Play("Empty");

        var playerSM = transform.GetComponentInParent<PlayerStateMachine>();
        playerSM.Weapon = null;
    }

    //TODO Add additional functionality.
    public void FillAbilitySlot(Weapon weapon)
    {
        weapon.AddAbility(weapon.Pool._ability[Random.Range(0, weapon.Pool._ability.Length)], weapon, gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.black;
        Vector3 position = CircleOrigin == null ? Vector3.zero : CircleOrigin.position;
        Gizmos.DrawWireSphere(position, Radius);
    }

    //TODO Add additional functionality.
    public void DetectColliders()
    {
        foreach (Collider2D collider in Physics2D.OverlapCircleAll(CircleOrigin.position, Radius))
        {
            Debug.Log(collider.name);
        }
    }

    public void PlayAnimation(AnimationClip clip)
    {
        AnimationClip idle = GetComponent<Animator>().GetCurrentAnimatorClipInfo(0)[0].clip;
        GetComponent<Animator>().Play(clip.name);
        StartCoroutine(ResetAnimation(clip.length, idle));
    }

    IEnumerator ResetAnimation(float time, AnimationClip idle)
    {
        yield return new WaitForSeconds(time);
        GetComponent<Animator>().Play(idle.name);
    }
}
