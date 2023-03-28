using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EntitySpawner : Singleton<EntitySpawner>
{
    #region Private Fields
    static GameObject _entityHolder;
    static GameObject _enemyHolder;
    #endregion

    protected override void OnAwake()
    {
        _entityHolder = new GameObject("Entities");
        _enemyHolder = new GameObject("Enemies");
        _enemyHolder.transform.parent = _entityHolder.transform;
    }

    public static GameObject SpawnEntity(GameObject entityOriginal, Vector2 position)
    {
        return Instantiate(entityOriginal, position, Quaternion.identity, _entityHolder.transform);
    }

    public static Enemy SpawnEnemy(Enemy enemyOriginal, Vector2 position)
    {
        return Instantiate(enemyOriginal, position, Quaternion.identity, _enemyHolder.transform);
    }
}
