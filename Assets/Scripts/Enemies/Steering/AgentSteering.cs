using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BulletHell.Enemies.Detection;

namespace BulletHell.Enemies.Steering
{
    [RequireComponent(typeof(AgentDetection))]
    public class AgentSteering : MonoBehaviour
    {
        [Header("Context Data")]
        public int Resolution = 16;
        public float ColliderRadius;
        [SerializeField] List<SteeringBehaviour> _behaviours = new List<SteeringBehaviour>();

        [Header("Agent Data")]
        [SerializeField] float speed = 3.5f;


        [HideInInspector] public Vector2[] Directions;
        ContextMap _interest, _danger;
        AgentDetection _detector;
        
        private void Awake()
        {
            _detector = GetComponent<AgentDetection>();
            _interest = new ContextMap(Resolution);
            _danger = new ContextMap(Resolution);
            CreateDirections();
        }

        private void Start()
        {
            InvokeRepeating(nameof(EvaluateBehaviors), 0, 0.1f);
        }

        private void EvaluateBehaviors()
        {
            _interest.Clear();
            _danger.Clear();

            foreach (SteeringBehaviour behaviour in _behaviours) {
                behaviour.GetSteering(_danger, _interest, this, _detector.Data);
            }
        }


        private void Update()
        {
            Vector2 moveDirection = (Vector3)ContextSolver.GetDirection(_danger, _interest, this);
            float angle = Vector2.SignedAngle(Vector2.right, moveDirection) - 90f;
            Vector3 targetRotation = new Vector3(0, 0, angle);
            var lookTo = Quaternion.Euler(targetRotation);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, lookTo, 100 * Time.deltaTime);
            transform.position += (Vector3)moveDirection * Time.deltaTime;
        }


        void CreateDirections()
        {
            Directions = new Vector2[Resolution];
            for (int i = 0; i < Resolution; i++) {
                float angle = i * Mathf.PI * 2 / Resolution;
                Directions[i] = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)).normalized;
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, ColliderRadius);
           
            if (_interest != null && _danger != null) {
                for (int i = 0; i < Resolution; i++) {
                    Gizmos.color = Color.green;
                    Gizmos.DrawLine(transform.position, (Vector2)transform.position + Directions[i] * _interest[i]);
                    Gizmos.color = Color.red;
                    Gizmos.DrawLine(transform.position, (Vector2)transform.position + Directions[i] * _danger[i]);
                }
            }
        }
    }
}
