using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaBar : MonoBehaviour
{
    [SerializeField] Image _image;
    [SerializeField]float _flashTime = 0;
    RectTransform _rectTransform;
    SpriteRenderer _spriteRenderer;
    int _currentStaminaCount;


    private void Awake()
    {
        _rectTransform = _image.GetComponent<RectTransform>();
        _rectTransform.sizeDelta = new Vector2(0, 0);
    }

    public void UpdateBar(int amount)
    {
        int amountToAdd = amount - _currentStaminaCount;

        if(amountToAdd == 0) { return; }

        _currentStaminaCount += amountToAdd;
        _rectTransform.sizeDelta = new Vector2(100 * _currentStaminaCount, 100);
        StartCoroutine(Flash());
    }

    IEnumerator Flash()
    {
        float timeElapsed = 0;
        while(timeElapsed < _flashTime) {
            yield return new WaitForEndOfFrame();
            timeElapsed += Time.deltaTime;
            _image.material.SetFloat("_FlashAmount", timeElapsed / _flashTime);
        }
        _image.material.SetFloat("_FlashAmount", 0);
    }
}
