using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BulletHell
{

    //Required for objects that are to be pooled.
    public interface IPoolable
    {
        public void ResetObject();
    }
}
