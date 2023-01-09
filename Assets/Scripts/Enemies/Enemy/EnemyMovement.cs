using BulletHell.Enemies.Steering;
using System.Collections.Generic;
using UnityEngine;
using BulletHell.Enemies.Detection;

namespace BulletHell.Enemies
{
    [RequireComponent(typeof(Enemy), typeof(Rigidbody2D))]
    public class EnemyMovement : MonoBehaviour
    {
        public bool DrawGizmos = false;
        [SerializeField] AgentSteering _agentSteering = new AgentSteering();

        public float Acceleration;
        public float MoveSpeed;

        Enemy _enemy;
        Rigidbody2D _rb;

        void Start()
        {
            _enemy = GetComponent<Enemy>();
            _rb = GetComponent<Rigidbody2D>();
            _agentSteering.Initialize(this);

            InvokeRepeating(nameof(EvaluateSteering), 0, 0.05f);
        }

        private void Update()
        {
            Move();
        }


        public void EvaluateSteering()
        {
            _agentSteering.EvaluateBehaviors(_enemy);
        }

        public void Move()
        {
            Vector2 MoveDirection = ContextSolver.GetDirection(_agentSteering);

   

            _rb.AddForce(MoveDirection * MoveSpeed);
            _rb.velocity = Vector3.ClampMagnitude(_rb.velocity, MoveSpeed);

        }

        private void OnDrawGizmos()
        {
            if (_agentSteering != null)
                _agentSteering.OnDrawGizmos();
        }
    }
}