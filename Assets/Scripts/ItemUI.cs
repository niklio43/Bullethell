using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemUI : MonoBehaviour
{
    [SerializeField] Item item;
    [SerializeField] TMP_Text title, amount;
    int _amount = 10;

    void Start()
    {
        GetComponent<Image>().sprite = item.Sprite;
        amount.text = "" + _amount;
        title.text = item.ItemName;
    }

    public void ConsumeItem()
    {
        if (_amount <= 0) return;
        _amount--;
        if(_amount == 0) { GetComponent<Image>().color = Color.grey; }
        amount.text = ""+_amount;
    }
}
