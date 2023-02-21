using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using BulletHell.StatusSystem;
using BulletHell;

namespace BulletHell.UI
{
    public class StatusEffectIcon : MonoBehaviour, IPoolable
    {
        [SerializeField] Image _mask;
        [SerializeField] Image _statusIcon;
        [SerializeField] TMP_Text _counter;
        StatusEffect _statusEffect;

        Pool<StatusEffectIcon> _pool;
        public void SetPool(Pool<StatusEffectIcon> pool) => _pool = pool;

        private void Awake()
        {
            _statusIcon.material = Instantiate(_statusIcon.material);
        }

        public void Initialize(StatusEffect statusEffect)
        {
            _statusEffect = statusEffect;
            _statusIcon.sprite = statusEffect.Icon;
            _statusEffect.OnExit += ResetObject;
            _statusEffect.OnTick += Tick;
            gameObject.SetActive(true);
        }

        private void Update()
        {
            if (_statusEffect == null) { return; }
            _counter.text = _statusEffect.GetStackCount().ToString();
            _mask.fillAmount = 1 - (_statusEffect.GetTimerCount() / _statusEffect.LifeTime);
        }

        public void ResetObject()
        {
            _statusIcon.sprite = null;
            gameObject.SetActive(false);
            _pool.Release(this);
        }

        public void Tick()
        {
            StartCoroutine(Flash());
        }

        IEnumerator Flash()
        {
            float timeElapsed = 0;
            while (timeElapsed < .1f) {
                yield return new WaitForEndOfFrame();
                timeElapsed += Time.deltaTime;
                _statusIcon.material.SetFloat("_FlashAmount", timeElapsed / .1f);
            }
            _statusIcon.material.SetFloat("_FlashAmount", 0);
        }

    }
}
