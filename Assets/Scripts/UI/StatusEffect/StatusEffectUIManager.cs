using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BulletHell;
using BulletHell.StatusSystem;

namespace BulletHell.UI
{
    public class StatusEffectUIManager : MonoBehaviour
    {
        [SerializeField] StatusEffectIcon _iconPrefab;
        Pool<StatusEffectIcon> _pool;

        private void Awake()
        {
            _pool = new Pool<StatusEffectIcon>(NewStatusEffect, 10);
        }

        StatusEffectIcon NewStatusEffect()
        {
            StatusEffectIcon newIcon = Instantiate(_iconPrefab);
            newIcon.transform.SetParent(transform);
            newIcon.gameObject.SetActive(false);
            newIcon.SetPool(_pool);
            return newIcon;
        }

        public void AddStatusEffect(StatusEffect statusEffect)
        {
            StatusEffectIcon icon = _pool.Get();
            icon.Initialize(statusEffect);
        }
    }
}
