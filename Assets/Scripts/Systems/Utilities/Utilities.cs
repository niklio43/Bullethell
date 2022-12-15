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
            float sin = Mathf.Sin(d * Mathf.Deg2Rad);
            float cos = Mathf.Cos(d * Mathf.Deg2Rad);

            float tx = v.x;
            float ty = v.y;

            v.y = (cos * tx) - (sin * ty);
            v.x = (sin * tx) - (cos * ty);

            return v;
        }

        public static Vector3 DirFromAngle(float angleInDegrees, float rotation)
        {
            angleInDegrees += rotation + 90;

            return new Vector3(Mathf.Cos(angleInDegrees * Mathf.Deg2Rad), Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0);
        }
    }
}