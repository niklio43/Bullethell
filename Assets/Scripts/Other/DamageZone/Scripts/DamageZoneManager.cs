using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BulletHell;

public class DamageZoneManager : Singleton<DamageZoneManager>
{
    #region Private Fields
    static ObjectPool<DamageZone> _zonePool;
    static DamageZone _zone;
    #endregion

    #region Private Methods
    protected override void OnAwake()
    {
        _zone = Resources.Load<DamageZone>("DamageZone");
        _zonePool = new ObjectPool<DamageZone>(Create, 100, "DamageZonePool", transform);
    }

    DamageZone Create()
    {
        DamageZone zone = Instantiate(_zone);
        zone.Initialize(_zonePool);
        return zone;
    }
    #endregion

    #region Public Methods
    public static DamageZone PlaceZone(Vector2 position, float size)
    {
        DamageZone zone = _zonePool.Get();
        zone.gameObject.SetActive(true);
        zone.transform.position = position;
        zone.Indicate(size);
        return zone;
    }
    #endregion
}
