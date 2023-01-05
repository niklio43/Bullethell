using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.VFX;

namespace BulletHell.Abilities
{
    public abstract class Ability : ScriptableObject
    {
        [SerializeField] Sprite _abilityIcon;
        public string _abilityName;
        public float coolDownTime = 0;
        public int maxAmount = 1;
        public int currentAmount;
        [SerializeField] VisualEffect _weaponVfx;
        public AnimationClip WeaponAttackAnimation;

        public float timer
        {
            get
            {
                if (timers.Count == 0) { return 0; }
                return timers[0];
            }
        }
        List<float> timers;

        public virtual void Initialize(WeaponController weaponController) { }
        public virtual void UnInitialize(WeaponController weaponController) { }

        public void Activate(InputAction.CallbackContext context)
        {
            if (currentAmount <= 0) return;
            if (!context.performed) return;

            DoAbility();

            //_weaponVfx.Play();

            currentAmount--;

            timers.Add(coolDownTime);
        }

        public abstract void DoAbility();

        public virtual void UpdateAbility(float dt)
        {
            for (int i = 0; i < timers.Count; i++)
            {
                timers[i] -= dt;
                if (timers[i] <= 0)
                {
                    currentAmount++;
                    timers.RemoveAt(i);
                    i--;
                }
            }
        }

    }
}