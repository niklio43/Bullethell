using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaminaBar : MonoBehaviour
{
    [SerializeField] GameObject _staminaIconPrefab;
    List<GameObject> _staminaIcons;

    private void Awake()
    {
        _staminaIcons = new List<GameObject>();
    }

    public void UpdateBar(int amount)
    {
        int amountToAdd = amount - _staminaIcons.Count;

        if(amountToAdd == 0) { return; }

        if(amountToAdd > 0) { AddStamina(amountToAdd); }
        else if(amountToAdd < 0) { RemoveStamina(Mathf.Abs(amountToAdd)); }
    }

    void AddStamina(int amount)
    {
        for (int i = 0; i < amount; i++) {
            GameObject newIcon = Instantiate(_staminaIconPrefab);
            newIcon.transform.parent = transform;
            _staminaIcons.Add(newIcon);
        }
    }

    void RemoveStamina(int amount)
    {
        for (int i = 0; i < amount; i++) {
            Destroy(_staminaIcons[i]);
            _staminaIcons[i] = null;
        }
    }
}
