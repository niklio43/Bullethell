using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BulletHell.Enemies.Detection;

namespace BulletHell.Enemies.Steering
{
    public abstract class SteeringBehaviour : ScriptableObject
    {
        public abstract void GetSteering(ContextMap danger, ContextMap interest, AgentSteering steering, DetectionData detectionData);
    }
}
