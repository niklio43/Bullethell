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

        public float timer
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
            if (currentAmount <= 0) return;
            DoAbility();

            currentAmount--;

            timers.Add(coolDownTime);
        }

        public abstract void DoAbility();

        public bool CanCast() => (currentAmount > 0);

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