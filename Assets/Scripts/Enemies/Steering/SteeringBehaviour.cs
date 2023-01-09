using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BulletHell.Enemies.Detection;

namespace BulletHell.Enemies.Steering
{
    public abstract class SteeringBehaviour : ScriptableObject
    {
        public abstract void GetSteering(AgentSteering steering, Enemy enemy);
    }
}
