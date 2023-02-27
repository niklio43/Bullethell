using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ForgeUI : Singleton<ForgeUI>
{
    public Slider ProgressBar;
    public bool IsUpgrading = false;

    private void Update()
    {
        if(!IsUpgrading) { ProgressBar.value = 0; return; }
        ProgressBar.value += Time.deltaTime;
    }
}
