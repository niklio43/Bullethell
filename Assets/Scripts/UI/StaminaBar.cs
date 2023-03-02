using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BulletHell.UI {
    public class StaminaBar : MonoBehaviour
    {
        [SerializeField] Image _image;
        [SerializeField] float _flashTime = 0;

        int _currentStaminaCount;

        RectTransform _rectTransform;
        SpriteRenderer _spriteRenderer;
        PlayerResources _playerResources;

        private void Awake()
        {
            _rectTransform = _image.GetComponent<RectTransform>();
            _rectTransform.sizeDelta = new Vector2(0, 0);
            _image.material = Instantiate(_image.material);
        }

        public void Initialize(PlayerResources playerResources)
        {
            _playerResources = playerResources;
            _playerResources.OnStaminaChanged += UpdateBar;
            UpdateBar();
        }

        void UpdateBar()
        {
            int amountToAdd = _playerResources.Stamina - _currentStaminaCount;
            if (amountToAdd == 0) return;
            _currentStaminaCount += amountToAdd;
            _rectTransform.sizeDelta = new Vector2(100 * _currentStaminaCount, 100);
            StartCoroutine(Flash());
        }

        IEnumerator Flash()
        {
            float timeElapsed = 0;
            while (timeElapsed < _flashTime) {
                yield return new WaitForEndOfFrame();
                timeElapsed += Time.deltaTime;
                _image.material.SetFloat("_FlashAmount", timeElapsed / _flashTime);
            }
            _image.material.SetFloat("_FlashAmount", 0);
        }
    }
}