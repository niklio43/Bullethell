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
    [CreateAssetMenu(fileName = "DashAbilityBehaviour", menuName = "Abilities/Custom Behaviours/New Dash Behaviour")]
    public class DashAbilityBehaviour : BaseAbilityBehaviour
    {
        BulletHell.Player.PlayerController _player;
        [Header("VFX")]
        [SerializeField] VisualEffectAsset _vfx;
        protected override void Perform()
        {
            _player = _ability.Owner.GetComponent<BulletHell.Player.PlayerController>();
            if(_player.Character.Stats["Stamina"].Get() <= 0) { return; }
            Dash(_ability.Owner);
        }

        public void Dash(GameObject owner)
        {
            Vector2 dir = _player.MovementInput.normalized;
            if (dir == Vector2.zero) return;

            Camera.main.Zoom(.2f, .5f);

            BulletHell.VFX.VFXManager.PlayBurst(_vfx, owner.transform.position, null, new VFXAttribute[] { new VFXFloat("Angle", Vector2.SignedAngle(new Vector2(dir.x, -dir.y), Vector2.left)) });

            _player.IsDashing = true;
            Debug.Log(dir);
            _player.Rb.AddForce(dir * _player.Character.Stats["DashDistance"].Value, ForceMode2D.Impulse);
            _player.UsedStamina(1);

            MonoInstance.Instance.StartCoroutine(CreateAfterImages(0.02f));

            MonoInstance.Instance.Invoke(() => ResetDash(), _player.DashTime);
        }

        IEnumerator CreateAfterImages(float timeBetween)
        {
            float timeElapsed = 0;
            float timeSinceLastImage = 0;
            while(timeElapsed < _player.DashTime)
            {
                yield return new WaitForFixedUpdate();
                timeElapsed += Time.fixedDeltaTime;
                timeSinceLastImage += Time.deltaTime;
                if(timeSinceLastImage > timeBetween)
                {
                    timeSinceLastImage = 0;
                    PlayerAfterImageSprite afterImage = _player.AfterImagePool.Get();
                    afterImage.gameObject.SetActive(true);
                    afterImage.Initialize(_player.transform, _player.GetComponent<SpriteRenderer>().sprite);
                }
            }
        }

        void ResetDash()
        {
            _player.IsDashing = false;
        }
    }
}