using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BulletHell.Player;
using BulletHell;
using UnityEngine.VFX;
using BulletHell.VFX;
using Bullet.CameraUtilities;

namespace BulletHell.Abilities
{
    [CreateAssetMenu(fileName = "ParryAbilityBehaviour", menuName = "Abilities/Custom Behaviours/New Parry Behaviour")]
    public class ParryAbilityBehaviour : BaseAbilityBehaviour
    {
        PlayerController _player;
        [Header("VFX")]
        [SerializeField] VisualEffectAsset _parryVfx;
        [SerializeField] VisualEffectAsset _dissipateVfx;
        [Header("Parry Radius")]
        [SerializeField, Range(0, 10)] float _radius = 5;
        [Header("Layers to parry")]
        [SerializeField] LayerMask layerMask;
        [Header("Stamina Cost")]
        [SerializeField, Range(0, 10)] int _staminaCost = 1;
        protected override void Perform()
        {
            _player = _ability.Owner.GetComponent<PlayerController>();

            if (_player.Character.Stats["Stamina"].Get() < _staminaCost) { return; }

            _player.IsParrying = true;

            _player.UsedStamina(_staminaCost);
            VFX.VFXManager.PlayBurst(_parryVfx, _player.transform.position, null);
            Camera.main.Zoom(.2f, .5f);

            Collider2D[] colliders = Physics2D.OverlapCircleAll(_player.transform.position, _radius, layerMask);

            if (colliders == null || colliders.Length == 0) { _player.IsParrying = false; return; }

            for (int i = 0; i < colliders.Length; i++)
            {
                VFX.VFXManager.PlayBurst(_dissipateVfx, colliders[i].gameObject.transform.position, null);
                MonoInstance.Instance.StartCoroutine(Parry(colliders[i].gameObject, 0.4f));
            }
        }

        IEnumerator Parry(GameObject attackObj, float duration)
        {
            attackObj.SetActive(false);
            Debug.Log("Parried: " + attackObj.name);

            yield return new WaitForSeconds(duration);

            _player.IsParrying = false;
        }
    }
}