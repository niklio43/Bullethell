using UnityEngine;

namespace BulletHell.Enemies.Steering
{
    public abstract class SteeringBehaviour : ScriptableObject
    {
        public abstract void GetSteering(AgentSteering steering, Enemy enemy);
    }
}
