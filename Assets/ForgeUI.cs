using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using BulletHell.Player;

public class ForgeUI : Singleton<ForgeUI>
{
    [SerializeField] Slider _progressBar;
    [SerializeField] TextMeshProUGUI _currentHealth;
    [SerializeField] PlayerController _playerController;
    [HideInInspector] public bool IsUpgrading = false;

    private void Update()
    {
        _currentHealth.text = string.Concat("Blood: ", _playerController.Character.Stats["Hp"].Get());

        if(!IsUpgrading) { _progressBar.value = 0; return; }
        _progressBar.value += Time.deltaTime;
    }
}
