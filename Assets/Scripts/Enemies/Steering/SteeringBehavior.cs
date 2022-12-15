using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BulletHell.Enemies.Detection;

namespace BulletHell.Enemies.Steering
{
    public abstract class SteeringBehavior
    {
        public abstract void GetSteering(ContextMap dangerMap, ContextMap interestMap, SteeringData steeringData, DetectionData detectionData);



    }
}
