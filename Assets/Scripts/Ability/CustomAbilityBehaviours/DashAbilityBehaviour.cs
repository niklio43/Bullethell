using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BulletHell.Player;
using BulletHell;
using UnityEngine.VFX;
using BulletHell.VFX;

namespace BulletHell.Abilities
{
    [CreateAssetMenu(fileName = "DashAbilityBehaviour", menuName = "Abilities/Custom Behaviours/New Dash Behaviour")]
    public class DashAbilityBehaviour : BaseAbilityBehaviour
    {
        BulletHell.Player.PlayerController _player;
        [Header("VFX")]
        [SerializeField] VisualEffectAsset _vfx;
        public override void Perform(GameObject owner)
        {
            _player = owner.GetComponent<BulletHell.Player.PlayerController>();
            Dash(owner);
        }

        public void Dash(GameObject owner)
        {
            Vector2 dir = _player.MovementInput.normalized;
            if (dir == Vector2.zero) return;

            BulletHell.VFX.VFXManager.PlayBurst(_vfx, owner.transform.position, null, new VFXAttribute[] { new VFXFloat("Angle", Vector2.SignedAngle(new Vector2(dir.x, -dir.y), Vector2.left)) });

            _player.IsDashing = true;

            _player.Rb.AddForce(dir * _player.Stats["DashDistance"].Value, ForceMode2D.Impulse);

            for (int i = 0; i < _player.AmountOfImages; i++)
            {
                PlayerAfterImageSprite afterImage = _player.AfterImagePool.Get();
                afterImage.gameObject.SetActive(true);
                afterImage.Initialize(i);
            }

            MonoInstance.Instance.Invoke(() => ResetDash(), _player.DashTime);
        }

        void ResetDash()
        {
            _player.IsDashing = false;
        }
    }
}