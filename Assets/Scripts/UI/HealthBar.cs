using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BulletHell.Stats;

namespace BulletHell.UI
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] Image _fill;
        [SerializeField] float _flashTime = .1f;

        Stat _healthStat;
        Slider _slider;

        private void Awake()
        {
            _slider = GetComponent<Slider>();
            _fill.material = Instantiate(_fill.material);
        }

        public void Initialize(Stat stat)
        {
            _healthStat = stat;
            _healthStat.OnValueChanged += UpdateBar;
            UpdateBar();
        }

        void UpdateBar()
        {
            float value = _healthStat.Get();
            if (value > _slider.maxValue) { _slider.maxValue = value; }
            _slider.value = value;

            StartCoroutine(Flash());
        }

        IEnumerator Flash()
        {
            float timeElapsed = 0;
            while (timeElapsed < _flashTime) {
                yield return new WaitForEndOfFrame();
                timeElapsed += Time.deltaTime;
                _fill.material.SetFloat("_FlashAmount", timeElapsed / _flashTime);
            }
            _fill.material.SetFloat("_FlashAmount", 0);
        }
    }
}
