using BulletHell.Map.Generation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BulletHell.Enemies.Collections;
using BulletHell.GameEventSystem;
using Pathfinding;

namespace BulletHell.Map
{
    public class LevelManager : MonoBehaviour
    {
        #region Public Fields
        public Room ActiveRoom => _activeRoom;

        public EnemyCollectionGroup EnemyCollectionGroup = null;

        #endregion

        #region Private Fields
        [SerializeField] GenerationConfig _config;
        [Header("Events")]
        [SerializeField] SOGameEvent OnPlayerMoved;
        [SerializeField] SOGameEvent OnCompletedMapGeneration;

        GenerationData _data;
        Room _activeRoom;
        #endregion

        #region Private Methods
        private void Start()
        {
            EnemyCollectionGroup = new EnemyCollectionGroup(_config.EnemyCollectionGroup);
            BeginGeneration();
        }

        public void PlayerEnterRoom(Component sender, object data)
        {
            if(data is not Room) { return; }
            Room room = data as Room;

            _activeRoom = room;
            OnPlayerMoved.Raise(this, room);
        }

        public void BeginGeneration()
        {
            Generator levelGenerator = new Generator(_config);
            levelGenerator.BeginGeneration(OnCompletedGeneration);
        }

        void OnCompletedGeneration(GenerationData data)
        {
            Debug.Log("Generation Completed");
            _data = data;

            foreach (Room room in _data.Rooms) {
                room.Initialize(this);
                room.transform.parent = transform;
            }

            CreateAstarBounds();
            OnCompletedMapGeneration.Raise(this, _data);
        }

        void CreateAstarBounds()
        {
            AstarData data = AstarPath.active.data;

            GridGraph gg = data.AddGraph(typeof (GridGraph)) as GridGraph;
            float nodeSize = 1f;
            int width = _data.Bounds.Width * (int)(1 / nodeSize);
            int depth = _data.Bounds.Height * (int)(1 / nodeSize);
            Debug.Log(new Vector3(_data.Bounds.Center.x, _data.Bounds.Center.y, 0));

            gg.center = new Vector3(_data.Bounds.Center.x, _data.Bounds.Center.y, 0) ;
            gg.SetDimensions(width, depth, nodeSize);
            gg.is2D = true;
            gg.collision.use2D = true;

            LayerMask mask = 1 << LayerMask.NameToLayer("Solid");

            gg.collision.mask = mask;
            AstarPath.active.Scan();
        }
        #endregion

        private void OnDrawGizmosSelected()
        {
            if (_data == null) return;

            Gizmos.color = Color.yellow;
            Vector3 center = new Vector3(_data.Bounds.Center.x, _data.Bounds.Center.y);
            Vector3 bounds = new Vector3(_data.Bounds.Width, _data.Bounds.Height);
            Gizmos.DrawWireCube(center, bounds);

        }
    }
}
