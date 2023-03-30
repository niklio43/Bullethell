using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace BulletHell.Abilities
{
    [System.Serializable]
    public abstract class BaseAbilityBehaviour : ScriptableObject
    {
        #region Private Fields
        [SerializeField] float _waitTime;
        [SerializeField] float _castTime = 0;

        [Header("Animation")]
        [SerializeField] protected List<AbilityAnimation> _animations;

        AbilityBehaviourState _state = AbilityBehaviourState.Idle;
        float _currentCastTime = 0;

        protected Ability _ability;
        protected Vector3 Target => _ability.Target;
        private enum AbilityBehaviourState
        {
            Idle,
            Channeling,
            Casting
        }
        #endregion

        #region Public Fields
        public int Id = -1;

        public float GetCastTime() => _castTime;
        public float GetWaitTime() => _waitTime;
        public float GetTotalTime() => _castTime + _waitTime;
        #endregion

        #region Private Methods
        protected virtual void Initialize() { }
        protected abstract void Perform();
        protected virtual void OnUpdate(float dt) { }

        async Task Channel()
        {
            _state = AbilityBehaviourState.Channeling;
            float timeElapsed = 0;

            try
            {
                while (timeElapsed < _waitTime)
                {
                    await UniTask.WaitForEndOfFrame(MonoInstance.Instance, _ability.CancellationToken);
                    timeElapsed += Time.deltaTime;
                }
            }
            catch
            {
                Debug.LogWarning("Ability Got Canceled");
                AbilityCanceled();
                return;
            }

            WhenCompletedChannel();
        }

        async Task Cast()
        {
            _state = AbilityBehaviourState.Casting;
            float timeElapsed = 0;

            try
            {
                while (timeElapsed < _castTime)
                {
                    await UniTask.WaitForEndOfFrame(MonoInstance.Instance, _ability.CancellationToken);
                    timeElapsed += Time.deltaTime;
                }
            }
            catch
            {
                Debug.LogWarning("Ability Got Canceled");
                AbilityCanceled();
                return;
            }

            Perform();
            _state = AbilityBehaviourState.Idle;
        }

        void AbilityCanceled()
        {
            _state = AbilityBehaviourState.Idle;
        }

        protected virtual void WhenCompletedChannel()
        {
            PlayAnimation();
        }

        protected virtual void PlayAnimation()
        {
            foreach (AbilityAnimation animation in _animations)
            {
                animation.Play();
            }
        }
        #endregion

        #region Public Methods
        public virtual void Uninitialize() { }

        public void InitializeBehaviour(Ability ability)
        {
            _ability = ability;

            foreach (AbilityAnimation animation in _animations) {
                animation.Initialize(ability);
            }
            Initialize();
        }

        public async Task Execute()
        {
            _state = AbilityBehaviourState.Channeling;

            await Channel();
            await Cast();

            _currentCastTime = 0;
        }

        public void UpdateBehaviour(float dt)
        {
            if (_state == AbilityBehaviourState.Channeling || _state == AbilityBehaviourState.Casting)
                _currentCastTime += dt;

            OnUpdate(dt);
        }
        #endregion
    }
}
