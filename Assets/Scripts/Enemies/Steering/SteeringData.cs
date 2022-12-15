using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BulletHell.Enemies.Steering
{
    public class SteeringData
    {
        public Vector2[] Directions;

        public Transform Transform;

        public SteeringData(int resolution, AgentSteering steering)
        {
            Transform = steering.transform;
            CreateDirections(resolution);
        }

        void CreateDirections(int resolution)
        {
            Directions = new Vector2[resolution];
            for (int i = 0; i < resolution; i++) {
                float angle = i * Mathf.PI * 2 / resolution;
                Directions[i] = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)).normalized;
            }
        }


    }
}
