using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace BulletHell.UI
{
    public class HealthBar : MonoBehaviour
    {
        #region Private Fields
        [SerializeField] Image _fill;
        [SerializeField] float _flashTime = .1f;
        [SerializeField] TextMeshProUGUI _healthText;
        Slider _slider;
        float _health, _maxHealth;
        #endregion

        private void Awake()
        {
            _slider = GetComponent<Slider>();
            _fill.material = Instantiate(_fill.material);
        }

        public void OnHealthChanged(Component sender, object data)
        {
            if(data is not float) { return; }
            _health = (float)data;
            UpdateBar(_health, _maxHealth);
        }

        public void OnMaxHealthChanged(Component sender, object data)
        {
            if (data is not float) { return; }
            _maxHealth = (float)data;
            UpdateBar(_health, _maxHealth);
        }

        void UpdateBar(float currentHealth, float maxHealth)
        {
            if(maxHealth > _slider.maxValue)
            {
                _slider.maxValue = currentHealth;
            }
            _slider.value = currentHealth;
            _healthText.text = string.Concat(currentHealth, "/", maxHealth);
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
