using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BulletHell.Abilities;
using UnityEngine.UI;
using TMPro;

public class AbilityHolder : MonoBehaviour
{
    [SerializeField] List<GameObject> _abilityUI;
    [SerializeField] List<Ability> _abilities = new List<Ability>();

    void Update()
    {
        for (int i = 0; i < _abilities.Count; i++)
        {
            _abilities[i].UpdateAbility(Time.deltaTime);
            _abilityUI[i].transform.GetChild(0).GetComponent<TMP_Text>().text = _abilities[i].GetCurrentAmount().ToString();
        }
    }

    public void AddAbility(Ability ability)
    {
        if(ability == null) { _abilities.Clear(); ValueChanged(); return; }
        _abilities.Add(ability);
        ValueChanged();
    }

    void ValueChanged()
    {
        if (_abilities.Count > 0) { SetGraphic(); return; }
        RemoveGraphic();
    }

    public void SetGraphic()
    {
        for (int i = 0; i < _abilities.Count; i++)
        {
            _abilityUI[i].name = _abilities[i].GetName();
            _abilityUI[i].GetComponent<Image>().sprite = _abilities[i].GetIcon();
        }
    }

    public void RemoveGraphic()
    {
        for (int i = 0; i < _abilityUI.Count; i++)
        {
            _abilityUI[i].name = "";
            _abilityUI[i].GetComponent<Image>().sprite = null;
            _abilityUI[i].transform.GetChild(0).GetComponent<TMP_Text>().text = "";

        }
    }
}
