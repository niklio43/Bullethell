using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BulletHell.Map.RoomEvents
{
    public class RoomEntitySpawnEvent : RoomEvent
    {
        [SerializeField] Vector2 _spawnPoint;
        [SerializeField] GameObject _entityPrefab;

        public override void StartEvent(Room room)
        {
            EntitySpawner.SpawnEntity(_entityPrefab, (Vector2)transform.position + _spawnPoint);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube((Vector2)transform.position + _spawnPoint, Vector2.one);
        }
    }
}
