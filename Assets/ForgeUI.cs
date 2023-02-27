using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ForgeUI : Singleton<ForgeUI>
{
    [SerializeField] Slider _progressBar;
    [SerializeField] TextMeshProUGUI _bloodTxt;
    [HideInInspector] public bool IsUpgrading = false;

    private void Update()
    {
        if(!IsUpgrading) { _progressBar.value = 0; return; }
        _progressBar.value += Time.deltaTime;
    }
}
