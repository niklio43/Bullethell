using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DisplayInventory : MonoBehaviour
{
    public Inventory Inventory;
    public int X_START;
    public int Y_START;
    public int X_SPACE_BETWEEN_ITEM;
    public int NUMBER_OF_COLUMN;
    public int Y_SPACE_BETWEEN_ITEMS;
    Dictionary<InventorySlot, GameObject> _itemsDisplayed = new Dictionary<InventorySlot, GameObject>();
    [SerializeField] GameObject _itemPrefab;

    void Start()
    {
        CreateDisplay();
    }

    void Update()
    {
        UpdateDisplay();
    }


    void CreateDisplay()
    {
        for (int i = 0; i < Inventory.Container.Items.Count; i++)
        {
            InventorySlot slot = Inventory.Container.Items[i];
            var obj = Instantiate(_itemPrefab, Vector3.zero, Quaternion.identity, transform);
            obj.GetComponent<RectTransform>().localPosition = GetPosition(i);
            obj.GetComponent<Image>().sprite = Inventory.database.GetItem[slot.Item.Id].Sprite;
            obj.GetComponentInChildren<TextMeshProUGUI>().text = slot.Amount.ToString("n0");
            _itemsDisplayed.Add(slot, obj);
        }
    }
    void UpdateDisplay()
    {
        for (int i = 0; i < Inventory.Container.Items.Count; i++)
        {
            InventorySlot slot = Inventory.Container.Items[i];
            if (_itemsDisplayed.ContainsKey(slot))
            {
                _itemsDisplayed[slot].GetComponentInChildren<TextMeshProUGUI>().text = slot.Amount.ToString("n0");
            }
            else
            {
                var obj = Instantiate(_itemPrefab, Vector3.zero, Quaternion.identity, transform);
                obj.GetComponent<RectTransform>().localPosition = GetPosition(i);
                obj.GetComponent<Image>().sprite = Inventory.database.GetItem[slot.Item.Id].Sprite;
                obj.GetComponentInChildren<TextMeshProUGUI>().text = slot.Amount.ToString("n0");
                _itemsDisplayed.Add(slot, obj);
            }
        }
    }

    public Vector3 GetPosition(int i)
    {
        return new Vector3(X_START + (X_SPACE_BETWEEN_ITEM * (i % NUMBER_OF_COLUMN)), Y_START + (-Y_SPACE_BETWEEN_ITEMS * (i / NUMBER_OF_COLUMN)), 0f);
    }

}
