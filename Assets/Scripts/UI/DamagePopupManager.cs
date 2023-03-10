using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BulletHell;

public class DamagePopupManager : Singleton<DamagePopupManager>
{
    public static DamagePopupManager GetInstance() => Instance;

    [SerializeField] Transform _damagePopupPrefab;

    ObjectPool<DamagePopup> _damagePopupPool;

    void Awake()
    {
        _damagePopupPool = new ObjectPool<DamagePopup>(CreateDamagePopup, 10, "DamagePopupPool");
    }

    DamagePopup CreateDamagePopup()
    {
        DamagePopup damagePopup = Instantiate(_damagePopupPrefab, transform.position, Quaternion.identity).GetComponent<DamagePopup>();

        damagePopup.Pool = _damagePopupPool;

        return damagePopup;
    }

    public void InsertIntoPool(float damage, Vector2 pos)
    {
        DamagePopup damagePopup = _damagePopupPool.Get();
        damagePopup.gameObject.SetActive(true);

        damagePopup.Setup(damage, pos);
    }
}
