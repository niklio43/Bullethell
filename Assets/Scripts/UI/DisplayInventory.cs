using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DisplayInventory : MonoBehaviour
{
    public Inventory Inventory;
    Dictionary<InventorySlot, GameObject> _itemsDisplayed = new Dictionary<InventorySlot, GameObject>();
    [SerializeField] GameObject _itemPrefab;
    List<GameObject> _slots = new List<GameObject>();

    void Start()
    {
        foreach(Transform child in gameObject.transform)
        {
            _slots.Add(child.gameObject);
        }
        CreateDisplay();
    }

    void Update()
    {
        UpdateDisplay();
    }


    void CreateDisplay()
    {
        for(int i = 0; i < Inventory.Container.Count; i++)
        {
            CreateObj(i);
        }
    }

    void UpdateDisplay()
    {
        for(int i = 0; i < Inventory.Container.Count; i++)
        {
            if (_itemsDisplayed.ContainsKey(Inventory.Container[i]))
            {
                _itemsDisplayed[Inventory.Container[i]].GetComponentInChildren<TextMeshProUGUI>().text = Inventory.Container[i].Amount.ToString("n0");
            }
            else
            {
                CreateObj(i);
            }
        }
    }

    void CreateObj(int i)
    {
        var obj = Instantiate(_itemPrefab, Vector3.zero, Quaternion.identity, _slots[i].transform);
        obj.GetComponent<Image>().sprite = Inventory.Container[i].Item.Sprite;
        obj.GetComponentInChildren<TextMeshProUGUI>().text = Inventory.Container[i].Amount.ToString("n0");
        obj.GetComponent<RectTransform>().localPosition = Vector3.zero;
        _itemsDisplayed.Add(Inventory.Container[i], obj);
    }

}
