using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BulletHell.Stats;
using TMPro;

namespace BulletHell.UI {
    public class StatUI : MonoBehaviour
    {
        [SerializeField] TMP_Text value;
        [SerializeField] string statName;

        Stat stat;

        public void Bind(Stats.Stats stats)
        {
            stat = stats[statName];
            stat.OnValueChanged += UpdateValue;

            UpdateValue();
        }

        public void UpdateValue()
        {
            value.text = $"{stat.Get()}";
        }
    }
}
