using BulletHell.Enemies.Steering;
using System.Collections.Generic;
using UnityEngine;
using BulletHell.Enemies.Detection;

namespace BulletHell.Enemies
{
    public class EnemyMovement : MonoBehaviour
    {
        public bool DrawGizmos = false;
        [SerializeField] float _moveSpeed;
        [SerializeField] AgentSteering _agentSteering = new AgentSteering();

        [HideInInspector] public Enemy Enemy;
        [HideInInspector] public Rigidbody2D Rb;
        [HideInInspector] public Collider2D Collider;
        [HideInInspector] public EnemyPathFinder PathFinder;

        public void Initialize(Enemy enemy)
        {
            Enemy = enemy;
            Rb = Enemy.GetComponent<Rigidbody2D>();
            Collider = GetComponent<Collider2D>();
            PathFinder = GetComponent<EnemyPathFinder>();

            PathFinder.Initialize(enemy);
            _agentSteering.Initialize();

            InvokeRepeating(nameof(EvaluateSteering), 0, 0.05f);
        }

        public void EvaluateSteering()
        {
            _agentSteering.EvaluateBehaviors(this);
        }

        public void Move()
        {
            if(!Enemy.CanMove) { return; }
            Vector2 MoveDirection = ContextSolver.GetDirection(_agentSteering);

            Rb.AddForce(MoveDirection * _moveSpeed);
            Rb.velocity = Vector3.ClampMagnitude(Rb.velocity, _moveSpeed);
        }

        private void OnDrawGizmosSelected()
        {
            if (_agentSteering != null && DrawGizmos)
                _agentSteering.OnDrawGizmos(transform);
        }
    }
}