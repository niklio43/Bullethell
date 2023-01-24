using System.Collections.Generic;
using UnityEngine;


namespace BulletHell.Abilities
{
    [CreateAssetMenu(fileName = "Ability", menuName = "Abilities/New Ability")]
    public sealed class Ability : ScriptableObject
    {
        public int Id = -1;
        [Header("General")]
        [SerializeField] Sprite _icon;
        [SerializeField] string _name;
        [SerializeField, TextArea] string _description;

        [Header("Settings")]
        [SerializeField] int _maxAmount;
        [SerializeField] float _coolDownTime;

        [Header("Ability Behaviours")]
        [SerializeField] List<BaseAbilityBehaviour> _behaviours;

        [Header("Animation")]
        [SerializeField] AnimationClip _clip;        #region Getters
        public bool CanCast() => (_currentAmount > 0);
        public float GetTimer() => (_timers.Count == 0) ? 0 : _timers[0];
        public int GetCurrentAmount => _currentAmount;
        public string GetName() => _name;
        public Sprite GetIcon() => _icon;
        public string GetDescription() => _description;
        #endregion

        GameObject _owner;
        GameObject _host;
        int _currentAmount;

        List<float> _timers;
        public void Initialize(GameObject owner, GameObject host = null)
        {
            _owner = owner;
            _host = (host == null) ? owner : host;

            _timers = new List<float>();
            _currentAmount = _maxAmount;

            for (int i = 0; i < _behaviours.Count; i++)
            {
                _behaviours[i] = Instantiate(_behaviours[i]);
                _behaviours[i].Initialize(this, owner, _host);
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

        public void Cast()
        {
            if (_currentAmount <= 0) return;
            DoAbility();

            _currentAmount--;

            _timers.Add(_coolDownTime);
        }

        void DoAbility()
        {
            if (_clip != null)
            {
                PlayAnimation(_clip.name);
                MonoInstance.GetInstance().Invoke(() => PlayAnimation("Idle"), _clip.length);
            }

            for (int i = 0; i < _behaviours.Count; i++)
            {
                _behaviours[i].Perform(_owner, _host);
            }
        }

        void PlayAnimation(string name)        {            _owner.GetComponent<Animator>().Play(name);        }

        public void UpdateAbility(float dt)
        {
            UpdateTimers(dt);
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
