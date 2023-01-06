using BulletHell.Enemies.Steering;
using System.Collections.Generic;
using UnityEngine;

namespace BulletHell.Enemies
{
    [RequireComponent(typeof(Enemy))]
    public class EnemyMovement : MonoBehaviour
    {
        Enemy _enemy;
        [SerializeField] AgentSteering _agentSteering = new AgentSteering();

        void Start()
        {
            _enemy = GetComponent<Enemy>();
            _agentSteering.Initialize(this, _enemy.Detection);

            InvokeRepeating(nameof(EvaluateSteering), 0, 0.05f);
        }

        public void EvaluateSteering()
        {
            _agentSteering.EvaluateBehaviors();
        }

        public void Move()
        {
            transform.position += (Vector3)_agentSteering.GetDirection() * _enemy.Stats.MoveSpeed * Time.deltaTime;
        }

        private void OnDrawGizmos()
        {
            if (_agentSteering != null)
                _agentSteering.OnDrawGizmos();
        }
    }
}