using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BulletHell.Enemies.Detection;

namespace BulletHell.Enemies.Steering
{
    [System.Serializable]
    public class AgentSteering
    {
        [Range(0, 10)] public float AvoidanceRadius = 2;
        [HideInInspector] public Vector2[] Directions;

        [SerializeField] int _resolution = 16;
        [SerializeField] List<SteeringBehaviour> _behaviours = new List<SteeringBehaviour>();

        public ContextMap Interest;
        public ContextMap Danger;

        public void Initialize()
        {
            Interest = new ContextMap(_resolution);
            Danger = new ContextMap(_resolution);
            CreateDirections(_resolution);
        }

        public void EvaluateBehaviors(Enemy enemy)
        {
            Interest.Clear();
            Danger.Clear();

            foreach (SteeringBehaviour behaviour in _behaviours) {
                behaviour.GetSteering(this, enemy);
            }
        }

        public Vector2 GetDirection()
        {
            return ContextSolver.GetDirection(this);
        }

        void CreateDirections(int resolution)
        {
            Directions = new Vector2[resolution];
            for (int i = 0; i < resolution; i++) {
                float angle = i * Mathf.PI * 2 / resolution;
                Directions[i] = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)).normalized;
            }
        }

        public void OnDrawGizmos(Transform transform)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, AvoidanceRadius);

            if(!Application.isPlaying) { return; } 
            Gizmos.color = new Color(.495f, .788f, .478f);
           
            if (Interest != null && Danger != null) {
                for (int i = 0; i < Directions.Length; i++) {
                    Gizmos.color = Color.green;
                    Gizmos.DrawLine(transform.position, (Vector2)transform.position + Directions[i] * Interest[i]);
                    Gizmos.color = Color.red;
                    Gizmos.DrawLine(transform.position, (Vector2)transform.position + Directions[i] * Danger[i]);
                }
            }
        }
    }
}
