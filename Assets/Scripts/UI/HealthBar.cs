using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace BulletHell.UI
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] Image _fill;
        [SerializeField] float _flashTime = .1f;
        [SerializeField] TextMeshProUGUI _healthText;
        PlayerResources _playerResources;

        Slider _slider;

        private void Awake()
        {
            _slider = GetComponent<Slider>();
            _fill.material = Instantiate(_fill.material);
        }

        public void Initialize(PlayerResources playerResources)
        {
            _playerResources = playerResources;
            _playerResources.OnHealthChanged += UpdateBar;
            UpdateBar();
        }

        void UpdateBar()
        {
            if(_playerResources.Health > _slider.maxValue)
            {
                _slider.maxValue = _playerResources.Health;
            }
            _slider.value = _playerResources.Health;
            _healthText.text = string.Concat(_playerResources.Health, "/", _playerResources.MaxHealth);
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
