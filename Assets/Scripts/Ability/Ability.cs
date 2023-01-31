using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;


namespace BulletHell.Abilities
{
    [CreateAssetMenu(fileName = "Ability", menuName = "Abilities/New Ability")]
    public class Ability : ScriptableObject
    {
        public int Id = -1;
        [Header("General")]
        [SerializeField] Sprite _icon;
        [SerializeField] string _name;
        [SerializeField, TextArea] string _description;

        [Header("Settings")]
        [SerializeField] int _maxAmount;
        [SerializeField] float _coolDownTime;

        [Header("Screenshake")]
        [SerializeField] float _screenShakeDuration = 0;
        [SerializeField] float _screenShakeAmplitude = 0;

        [Header("Ability Behaviours")]
        [SerializeField] List<BaseAbilityBehaviour> _behaviours;

        [Header("Status effect")]        [SerializeField] StatusEffectData _data;        #region Getters
        public bool CanCast() => (_currentAmount > 0);
        public float GetTimer() => (_timers.Count == 0) ? 0 : _timers[0];
        public int GetCurrentAmount => _currentAmount;
        public string GetName() => _name;
        public Sprite GetIcon() => _icon;
        public string GetDescription() => _description;
        #endregion

        AbilityState _abilityState = AbilityState.Idle;
        public GameObject Owner { get; private set; }
        public GameObject Host { get; private set; }
        int _currentAmount;

        List<float> _timers;

        public enum AbilityState
        {
            Idle,
            Channeling,
            Casting,
        }

        public void Initialize(GameObject owner, GameObject host = null)
        {
            this.Owner = owner;
            this.Host = (host == null) ? owner : host;

            _timers = new List<float>();
            _currentAmount = _maxAmount;

            for (int i = 0; i < _behaviours.Count; i++) {
                _behaviours[i] = Instantiate(_behaviours[i]);
                _behaviours[i].InitializeBehaviour(this);
            }
        }

        public void Uninitialize()        {
            _timers = null;
            _currentAmount = 0;

            for (int i = 0; i < _behaviours.Count; i++)
            {
                _behaviours[i].Uninitialize();
            }
        }

        public async void Cast(Action castDelegate = null)
        {
            if (_currentAmount <= 0 || _abilityState == AbilityState.Channeling) return;
            await DoAbility();

            _abilityState = AbilityState.Casting;
            Camera.main.Shake(_screenShakeDuration, _screenShakeAmplitude);
            castDelegate?.Invoke();
            _abilityState = AbilityState.Idle;
        }

        async Task DoAbility()
        {            _abilityState = AbilityState.Channeling;            await Task.WhenAll(_behaviours.Select(i => i.Execute()));
    

            _currentAmount--;            _timers.Add(_coolDownTime);
        }

        public void UpdateAbility(float dt)
        {
            UpdateTimers(dt);

            foreach (BaseAbilityBehaviour behaviour in _behaviours) {
                behaviour.UpdateBehaviour(dt);
            }
        }

        void UpdateTimers(float dt)
        {
            for (int i = 0; i < _timers.Count; i++)
            {
                _timers[i] -= dt;
                if (_timers[i] <= 0)
                {
                    _currentAmount++;
                    _timers.RemoveAt(i);
                    i--;
                }
            }
        }
    }
}
