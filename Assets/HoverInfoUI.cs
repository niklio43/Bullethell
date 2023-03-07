using BulletHell.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HoverInfoUI : MonoBehaviour
{
    public Image Icon;
    public TextMeshProUGUI ItemName;
    public TextMeshProUGUI ItemType;
    public TextMeshProUGUI ItemRarity;

    void Awake()
    {
        transform.SetParent(PlayerUI.Instance.transform);
    }
}
