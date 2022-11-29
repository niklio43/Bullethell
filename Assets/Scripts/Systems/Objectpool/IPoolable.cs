using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BulletHell
{
    public interface IPoolable
    {
        public void ResetObject();
        public void Dispose();
    }
}
