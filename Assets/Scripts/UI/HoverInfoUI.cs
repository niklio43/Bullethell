using BulletHell.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HoverInfoUI : MonoBehaviour
{
    [Header("Weapon info")]
    public Image Icon;
    public TextMeshProUGUI ItemName;
    public TextMeshProUGUI ItemType;
    public TextMeshProUGUI ItemRarity;

    [Header("Ability info")]
    public GameObject AbilityParent;
    public AbilityInfo AbilityPrefab;

    void Awake()
    {
        transform.SetParent(PlayerUI.Instance.transform);
    }
}
