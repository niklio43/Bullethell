using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EntitySpawner : Singleton<EntitySpawner>
{
    #region Private Fields
    static GameObject _entityHolder;
    static GameObject _enemyHolder;
    #endregion

    #region Private Methods

    private static void Validate()
    {
        if(_entityHolder == null) {
            _entityHolder = new GameObject("Entities");
            _enemyHolder = new GameObject("Enemies");
            _enemyHolder.transform.parent = _entityHolder.transform;
        }
    }

    #endregion

    #region Public Methods
    public static GameObject SpawnEntity(GameObject entityOriginal, Vector2 position)
    {
        Validate();
        return Instantiate(entityOriginal, position, Quaternion.identity, _entityHolder.transform);
    }

    public static Enemy SpawnEnemy(Enemy enemyOriginal, Vector2 position)
    {
        Validate();
        return Instantiate(enemyOriginal, position, Quaternion.identity, _enemyHolder.transform);
    }
    #endregion
}
