using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.VFX;

namespace BulletHell.Abilities
{
    public abstract class Ability : ScriptableObject
    {
        [SerializeField] Sprite _abilityIcon;
        public string AbilityName;
        public float CoolDownTime = 0;
        public int MaxAmount = 1;
        public int CurrentAmount;
        public float Damage;

        public float Timer
        {
            get
            {
                if (timers.Count == 0) { return 0; }
                return timers[0];
            }
        }
        List<float> timers;

        public virtual void Initialize(GameObject owner) { }
        public virtual void UnInitialize(GameObject owner) { }

        public void Activate(InputAction.CallbackContext context)
        {
            if (!context.performed) return;

            Activate();
        }

        public void Activate()
        {
            if (CurrentAmount <= 0) return;
            DoAbility();

            CurrentAmount--;

            timers.Add(CoolDownTime);
        }

        public abstract void DoAbility();

        public bool CanCast() => (CurrentAmount > 0);

        public virtual void UpdateAbility(float dt)
        {
            for (int i = 0; i < timers.Count; i++)
            {
                timers[i] -= dt;
                if (timers[i] <= 0)
                {
                    CurrentAmount++;
                    timers.RemoveAt(i);
                    i--;
                }
            }
        }

    }
}