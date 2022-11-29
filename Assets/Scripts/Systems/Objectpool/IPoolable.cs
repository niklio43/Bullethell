using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BulletHell.ObjectPool
{
    public interface IPoolable
    {
        public void ResetObject();
    }
}
