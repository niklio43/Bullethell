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
        [SerializeField] VisualEffectAsset _vfx;
        [Header("Parry Radius")]
        [SerializeField, Range(0, 10)] float _radius = 5;
        [Header("Layers to parry")]
        [SerializeField] LayerMask layerMask;
        [Header("Stamina Cost")]
        [SerializeField, Range(0, 100)] float _staminaCost = 10;
        protected override void Perform()
        {
            _player = _ability.Owner.GetComponent<BulletHell.Player.PlayerController>();
            _player.IsParrying = true;

            if (_player.Character.Stats["Stamina"].Get() < _staminaCost) { return; }

            //_player.GetComponent<Animator>().Play("Parry");

            Collider2D[] colliders = Physics2D.OverlapCircleAll(_player.transform.position, _radius, layerMask);

            if (colliders == null) { return; }

            //BulletHell.VFX.VFXManager.PlayBurst(_vfx, _ability.Owner.transform.position, null);
            Camera.main.Zoom(.2f, .5f);

            for (int i = 0; i < colliders.Length; i++)
            {
                //colliders[i].GetComponent<Animator>().Play("Dissipate");
                MonoInstance.Instance.StartCoroutine(Parry(colliders[i].gameObject, 0/*colliders[i].GetComponent<Animator>().GetCurrentAnimatorClipInfo(0).Length*/));
            }

        }

        IEnumerator Parry(GameObject attackObj, float time)
        {
            yield return new WaitForSeconds(time);
            attackObj.SetActive(false);
            //_player.Character.Stats["Stamina"].Value -= _staminaCost;
            Debug.Log("Parried: " + attackObj.name);
        }
    }
}