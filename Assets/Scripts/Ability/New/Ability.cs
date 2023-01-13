using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

namespace BulletHell.Abilities.New
{
    [CreateAssetMenu(fileName = "Ability", menuName = "Abilities/New Ability")]
    public sealed class Ability : ScriptableObject
    {
        [Header("General")]
        [SerializeField] Sprite _icon;
        [SerializeField] string _name;
        [SerializeField, TextArea] string _description;

        [Header("Settings")]
        [SerializeField] int _maxAmount;
        [SerializeField] float _coolDownTime;

        [Header("Ability Behaviours")]
        [SerializeField] List<BaseAbilityBehaviour> behaviours;

        [Header("Animation")]
        [SerializeField] AnimationClip _clip;

        [Header("VFX")]
        [SerializeField] VisualEffectAsset _vfx;

        GameObject _owner;
        int _currentAmount;

        List<float> _timers;
        public void Initialize(GameObject owner)
        {
            _owner = owner;
            _timers = new List<float>();

            foreach (BaseAbilityBehaviour behaviour in behaviours) {
                behaviour.Initialize(this, owner);
            }
        }
        public void Unitilize() {

            foreach (BaseAbilityBehaviour behaviour in behaviours) {
                behaviour.Unitialize();
            }
        }

        public void Cast()
        {
            if (_currentAmount <= 0 ) return;
            DoAbility();

            _currentAmount--;

            _timers.Add(_coolDownTime);
        }
        void DoAbility()
        {
            _owner.GetComponent<Animator>().Play(_clip.name);

            foreach (BaseAbilityBehaviour behaviour in behaviours) {
                behaviour.Perform(_owner);
            }
        }

        public void UpdateAbility(float dt)
        {
            UpdateTimers(dt);
        }

        void UpdateTimers(float dt)
        {
            for (int i = 0; i < _timers.Count; i++) {
                _timers[i] -= dt;
                if (_timers[i] <= 0) {
                    _currentAmount++;
                    _timers.RemoveAt(i);
                    i--;
                }
            }
        }

        #region Getters
        public bool CanCast() => (_currentAmount > 0);
        public float GetTimer() => (_timers.Count == 0) ? 0 : _timers[0];
        public string GetName() => _name;
        public string GetDescription() => _description;
        #endregion

    }
}
