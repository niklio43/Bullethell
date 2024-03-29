using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BulletHell.Enemies.Steering
{
    public static class ContextSolver
    {
        #region Public Methods
        public static Vector2 GetDirection(AgentSteering steering)
        {
            Vector2 direction = Vector2.zero;
            for (int i = 0; i < steering.Directions.Length; i++) {
                direction += steering.Directions[i] * Mathf.Clamp01(steering.Interest[i] - steering.Danger[i]);
            }

            direction.Normalize();
            return direction;
        }
        #endregion
    }
}
