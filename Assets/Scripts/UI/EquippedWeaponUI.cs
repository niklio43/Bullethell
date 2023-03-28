using BulletHell;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EquippedWeaponUI : Singleton<EquippedWeaponUI>
{
    #region Private Fields
    [SerializeField] Image _primaryWeapon;
    [SerializeField] Image _secondaryWeapon;
    #endregion

    #region Public Methods
    public void SetWeapon(Sprite icon, bool active, bool primary)
    {
        Image image;
        if (primary) { image = _primaryWeapon; }
        else { image = _secondaryWeapon; }

        image.sprite = icon;
        image.SetNativeSize();
        image.color = Color.white;
        if (!active) { image.color = Color.clear; }
    }

    public void SetActiveWeapon(bool primary)
    {
        Image active;
        Image disable;
        if (primary) { active = _primaryWeapon; disable = _secondaryWeapon; }
        else { active = _secondaryWeapon; disable = _primaryWeapon; }

        active.transform.GetChild(0).gameObject.SetActive(false);
        StartCoroutine(ChangeScale(active.GetComponent<RectTransform>(), 1.2f, 0.2f));
        disable.transform.GetChild(0).gameObject.SetActive(true);
        StartCoroutine(ChangeScale(disable.GetComponent<RectTransform>(), 1, 0.2f));
    }
    #endregion

    #region Private Methods
    IEnumerator ChangeScale(RectTransform rect, float scale, float time)
    {
        float startScale = rect.localScale.x;
        float timeElapsed = 0;
        while (timeElapsed < time)
        {
            yield return new WaitForEndOfFrame();
            timeElapsed += Time.deltaTime;

            float tempScale = Easing.EaseInOut(startScale, scale, timeElapsed / time);
            rect.localScale = Vector2.one * tempScale;
        }
        rect.localScale = Vector2.one * scale;
    }
    #endregion
}
