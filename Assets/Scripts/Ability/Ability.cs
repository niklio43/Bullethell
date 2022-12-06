using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
namespace BulletHell.Abilities
{
    [CreateAssetMenu(fileName = "New Ability", menuName = "Abilities/Ability")]
    public class Ability : ScriptableObject
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

        void Activate(InputAction.CallbackContext context)
        {
            if (currentAmount <= 0) return;
            if (!context.performed) return;

            DoAbility();
            currentAmount--;

            timers.Add(coolDownTime);
        }

        public void DoAbility() { }

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