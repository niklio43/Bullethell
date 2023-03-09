using BulletHell;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EquippedWeaponUI : Singleton<EquippedWeaponUI>
{
    [SerializeField] Image _primaryWeapon;
    [SerializeField] Image _secondaryWeapon;


    public Image PrimaryWeapon => _primaryWeapon;
    public Image SecondaryWeapon => _secondaryWeapon;

    public void SetWeapon(Image image, Sprite icon, bool active)
    {
        image.sprite = icon;
        image.SetNativeSize();
        image.color = Color.white;
        if (!active) { image.color = Color.clear; }
    }

    public void SetActiveWeapon(Image active, Image disable)
    {
        active.transform.GetChild(0).gameObject.SetActive(false);
        StartCoroutine(ChangeScale(active.GetComponent<RectTransform>(), 1.2f, 0.2f));
        disable.transform.GetChild(0).gameObject.SetActive(true);
        StartCoroutine(ChangeScale(disable.GetComponent<RectTransform>(), 1, 0.2f));
    }

    IEnumerator ChangeScale(RectTransform rect, float scale, float time)
    {
        float startScale = rect.localScale.x;
        float timeElapsed = 0;
        while(timeElapsed < time)
        {
            yield return new WaitForEndOfFrame();
            timeElapsed += Time.deltaTime;

            float tempScale = Easing.EaseInOut(startScale, scale, timeElapsed/time);
            rect.localScale = Vector2.one * tempScale;
        }
        rect.localScale = Vector2.one * scale;
    }
}
