using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

namespace BulletHell.Enemies.Steering
{
    public class EnemyPathFinder : MonoBehaviour
    {
        [SerializeField] float _targetDistanceThreshold = 1;
        [SerializeField] float _waypointDistanceThreshold = 1;

        Enemy _owner;
        Seeker _seeker;
        Path _currentPath;

        public PathState State = PathState.NoPath;
        int _currentWaypoint;

        public enum PathState
        {
            NoPath,
            HasPath,
            PathError
        }

        private void Awake()
        {
            _seeker = GetComponent<Seeker>();
        }

        public void Initialize(Enemy owner)
        {
            _owner = owner;
        }

        public Vector3 GetCurrentPathNode()
        {
            if (State == PathState.NoPath || State == PathState.PathError) { return transform.position; }
            return _currentPath.vectorPath[_currentWaypoint];
        }

        public Vector3 GetPathNode(int index)
        {
            if (State == PathState.NoPath || State == PathState.PathError) { return transform.position; }
            return _currentPath.vectorPath[index];
        }

        public void UpdatePathTraversal()
        {
            CheckPathValidity();
            if(State == PathState.NoPath) { return; }


            float d = Vector2.Distance(transform.position, GetPathNode(_currentWaypoint));
            if (d <= _waypointDistanceThreshold) {
                if (_currentWaypoint + 1 < _currentPath.vectorPath.Count) {
                    _currentWaypoint++;
                }
                else {
                    ReachedEndOfPath();
                }
            }
        }

        void CheckPathValidity()
        {
            if (_owner.Target == null) { return; }
            if (_currentPath == null || State == PathState.NoPath) { 
                SeekNewPath(); 
                return; 
            }

            Vector3 target = GetPathNode(_currentPath.vectorPath.Count - 1);
            float distance = Vector3.Distance(_owner.Target.position, target);
            if (distance > _targetDistanceThreshold) SeekNewPath();
        }

        void SeekNewPath()
        {
            if (_owner.Target == null) { return; }
            if (_currentPath != null) _currentPath.Release(this);
            _currentPath = null;
            State = PathState.NoPath;
            _seeker.StartPath(transform.position, _owner.Target.position, OnPathComplete);
        }

        void ReachedEndOfPath()
        {
            if(_currentPath == null) { return; }
            _currentPath.Release(this);
            _currentPath = null;
            State = PathState.NoPath;
        }

        void OnPathComplete(Path p)
        {
            if (p.error) {
                Debug.Log($"An error occured while calculating path: {p.errorLog}");
                State = PathState.PathError;
            }
            else {
                State = PathState.HasPath;
                p.Claim(this);
                _currentPath = p;
                _currentWaypoint = 0;
            }
        }
    }
}
