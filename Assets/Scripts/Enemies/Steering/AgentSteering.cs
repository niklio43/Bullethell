using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BulletHell.Enemies.Detection;

namespace BulletHell.Enemies.Steering
{
    [RequireComponent(typeof(AgentDetection))]
    public class AgentSteering : MonoBehaviour
    {
        [SerializeField] int resolution = 16;

        SteeringData _data;
        ContextMap _interest, _danger;
        AgentDetection _detector;
        

        private void Awake()
        {
            _detector = GetComponent<AgentDetection>();
            _data = new SteeringData(resolution, this);
            _interest = new ContextMap(resolution, "Interest Map");
            _danger = new ContextMap(resolution, "Danger Map");
        }


        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.white;
            if(_data == null) { return; }
            foreach (Vector2 direction in _data.Directions) {
                Gizmos.DrawLine(transform.position, (Vector2)transform.position + direction);
            }
        }
    }
}
