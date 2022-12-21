using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BulletHell.Enemies.Steering
{
    public static class ContextSolver
    {
        public static Vector2 GetDirection(ContextMap danger, ContextMap interest, AgentSteering steering)
        {
            Vector2 direction = Vector2.zero;
            for (int i = 0; i < steering.Resolution; i++) {
                direction += steering.Directions[i] * Mathf.Clamp01(interest[i] - danger[i]);
            }

            direction.Normalize();
            return direction;
        }
    }
}
