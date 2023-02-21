using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BulletHell.Stats;

namespace BulletHell.UI
{
    public class StatManagerUI : MonoBehaviour
    {
        [SerializeField] List<StatUI> _statList;

        public void Initilize(Stats.Stats stats)
        {
            foreach (StatUI stat in _statList) {
                stat.Bind(stats);
            }
        }
    }
}
