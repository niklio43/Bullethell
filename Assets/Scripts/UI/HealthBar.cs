using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] Image _fill;
    Slider _slider;

    [SerializeField] float _flashTime = .1f;

    private void Awake()
    {
        _slider = GetComponent<Slider>();
        _fill.material = Instantiate(_fill.material);
    }

    public void UpdateBar(float value)
    {
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
