using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
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

        public static T Copy<T>(T other)
        {
            using (MemoryStream ms = new MemoryStream()) {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(ms, other);
                ms.Position = 0;
                return (T)formatter.Deserialize(ms);
            }
        }

        public static Coroutine Invoke(this MonoBehaviour mb, Action action, float timeInSeconds)
        {
            return mb.StartCoroutine(InvokeRoutine(action, timeInSeconds));
        }

        private static IEnumerator InvokeRoutine(Action action, float delay)
        {
            yield return new WaitForSeconds(delay);
            action();
        }
    }
}