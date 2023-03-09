using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using BulletHell.InventorySystem;
using UnityEngine.UI;
using System;

public class HoverInfo : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    float _timeToWait = 0.5f;
    InventoryItemData _item;
    Color _hoverColor;

    void Start()
    {
        _hoverColor = new Color(0.8f, 0.8f, 0.8f, 1);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (GetComponent<DraggableItem>().IsDragging) { StopAllCoroutines(); return; }
        transform.parent.GetComponent<Image>().CrossFadeColor(_hoverColor, _timeToWait, true, false);
        _item = transform.parent.GetComponent<InventorySlotUI>().AssignedInventorySlot.ItemData;
        if (_item == null) { return; }
        StopAllCoroutines();
        StartCoroutine(StartTimer());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if(GetComponent<DraggableItem>().IsDragging) { StopAllCoroutines(); return; }
        transform.parent.GetComponent<Image>().CrossFadeColor(Color.white, _timeToWait, true, false);
        StopAllCoroutines();
        HoverInfoManager.Instance.HideInfo();
    }

    void ShowMessage()
    {
        HoverInfoManager.Instance.ShowInfo(_item);
    }

    IEnumerator StartTimer()
    {
        yield return new WaitForSeconds(_timeToWait);
        ShowMessage();
    }
}
