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
        BulletHell.Player.PlayerController _player;
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
            _player = _ability.Owner.GetComponent<BulletHell.Player.PlayerController>();

            if (_player.Character.Stats["Stamina"].Get() < _staminaCost) { return; }

            _player.IsParrying = true;

            BulletHell.VFX.VFXManager.PlayBurst(_parryVfx, _ability.Owner.transform.position, null);

            Collider2D[] colliders = Physics2D.OverlapCircleAll(_player.transform.position, _radius, layerMask);

            if (colliders == null) { return; }

            Camera.main.Zoom(.2f, .5f);

            for (int i = 0; i < colliders.Length; i++)
            {
                BulletHell.VFX.VFXManager.PlayBurst(_dissipateVfx, _ability.Owner.transform.position, null);
                Parry(colliders[i].gameObject);
            }

        }

        void Parry(GameObject attackObj)
        {
            attackObj.SetActive(false);
            _player.UsedStamina(_staminaCost);
            Debug.Log("Parried: " + attackObj.name);
        }
    }
}