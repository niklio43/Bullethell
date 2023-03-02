using System.Collections;
using UnityEngine;
using BulletHell.Player;
using UnityEngine.VFX;
using BulletHell.VFX;
using BulletHell.CameraUtilities;

namespace BulletHell.Abilities
{
    [CreateAssetMenu(fileName = "DashAbilityBehaviour", menuName = "Abilities/Custom Behaviours/New Dash Behaviour")]
    public class DashAbilityBehaviour : BaseAbilityBehaviour
    {
        PlayerController _player;
        [Header("VFX")]
        [SerializeField] VisualEffectAsset _vfx;
        [Header("Stamina Cost")]
        [SerializeField, Range(0, 10)] int _staminaCost = 1;
        protected override void Perform()
        {
            _player = _ability.Owner.GetComponent<PlayerController>();
            //FIX if (_player.Character.Stats["Stamina"].Get() < _staminaCost) { return; }
            Dash(_ability.Owner);
        }

        public void Dash(GameObject owner)
        {
            Vector2 dir = _player.PlayerMovement.MovementInput.normalized;
            if (dir == Vector2.zero) return;

            Camera.main.Zoom(.2f, .5f);

            VFX.VFXManager.PlayBurst(_vfx, owner.transform.position, null, new VFXAttribute[] { new VFXFloat("Angle", Vector2.SignedAngle(new Vector2(dir.x, -dir.y), Vector2.left)) });

            _player.PlayerAbilities.IsDashing = true;
            //FIX _player.PlayerMovement.Rb.AddForce(dir * _player.Character.Stats["DashDistance"].Value, ForceMode2D.Impulse);

            MonoInstance.Instance.StartCoroutine(CreateAfterImages(0.02f));

            MonoInstance.Instance.Invoke(() => ResetDash(), _player.PlayerAbilities.DashTime);
        }

        IEnumerator CreateAfterImages(float timeBetween)
        {
            float timeElapsed = 0;
            float timeSinceLastImage = 0;
            while(timeElapsed < _player.PlayerAbilities.DashTime)
            {
                yield return new WaitForFixedUpdate();
                timeElapsed += Time.fixedDeltaTime;
                timeSinceLastImage += Time.deltaTime;
                if(timeSinceLastImage > timeBetween)
                {
                    timeSinceLastImage = 0;
                    PlayerAfterImageSprite afterImage = _player.PlayerAbilities.AfterImagePool.Get();
                    afterImage.gameObject.SetActive(true);
                    afterImage.Initialize(_player.transform, _player.GetComponent<SpriteRenderer>().sprite);
                }
            }
        }

        void ResetDash()
        {
            _player.PlayerAbilities.IsDashing = false;
        }
    }
}