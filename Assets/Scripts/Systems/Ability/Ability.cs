using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;using BulletHell.CameraUtilities;using System.Threading;
using Cysharp.Threading.Tasks;

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

        int _currentAmount;
        #region Getters
        public string GetName() => _name;
        public string GetDescription() => _description;
        public int GetCurrentAmount() => _currentAmount;
        public bool CanCast() => (_currentAmount > 0 && _abilityState == AbilityState.Idle);
        public float GetTimer() => (_timers.Count == 0) ? 0 : _timers[0];
        public Sprite GetIcon() => _icon;
        #endregion

        public GameObject Owner { get; private set; }
        public GameObject Host { get; private set; }
        public CancellationToken CancellationToken => _cancellationTokenSource.Token;
        CancellationTokenSource _cancellationTokenSource;
        public int MaxAmount { get { return _maxAmount; } }
        
        AbilityState _abilityState = AbilityState.Idle;
        List<float> _timers;

        public Vector3 Target { get; private set; } = default(Vector3);

        public enum AbilityState
        {
            Idle,
            Channeling,
            Casting,
            Canceled
        }

        public void Initialize(GameObject owner, GameObject host = null)
        {
            _cancellationTokenSource = new CancellationTokenSource();
            Owner = owner;
            Host = (host == null) ? owner : host;

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

        public async void Cast(Vector3 target, Action castDelegate = null)
        {
            if (_currentAmount <= 0 || _abilityState == AbilityState.Channeling) return;
            Target = target;
            await DoAbility();

            if (_abilityState != AbilityState.Canceled) {
                Camera.main.Shake(_screenShakeDuration, _screenShakeAmplitude);
                _abilityState = AbilityState.Casting;
                castDelegate?.Invoke();
            }
            _abilityState = AbilityState.Idle;
        }

        async Task DoAbility()
        {            _abilityState = AbilityState.Channeling;            await Task.WhenAll(_behaviours.Select(i => i.Execute()));
    

            _currentAmount--;            _timers.Add(_coolDownTime);
        }

        public void Cancel()
        {
            _abilityState = AbilityState.Canceled;
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource.Dispose();
            _cancellationTokenSource = new CancellationTokenSource();
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
