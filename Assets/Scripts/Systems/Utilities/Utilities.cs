using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BulletHell
{
    public static class Utilities {

        //Rotates a vector (v) by the given amount in degrees (d).
        public static Vector2 Rotate(Vector2 v, float d)
        {
            v = Quaternion.AngleAxis(d, Vector3.forward) * v;

            return v;
        }

        public static Vector3 DirFromAngle(float angleInDegrees, float rotation)
        {
            angleInDegrees += rotation + 90;

            return new Vector3(Mathf.Cos(angleInDegrees * Mathf.Deg2Rad), Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0);
        }
    }
}