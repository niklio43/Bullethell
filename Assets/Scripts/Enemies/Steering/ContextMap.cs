using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BulletHell.Enemies.Steering
{
    public class ContextMap
    {
        float[] _values;

        public float this[int index] => _values[index];

        public ContextMap(int resolution, string name)
        {
            _values = new float[resolution];
        }
    }
}
