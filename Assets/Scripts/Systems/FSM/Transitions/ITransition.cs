using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BulletHell.FiniteStateMachine
{
    public interface ITransition 
    {
        #region Private Fields
        string Name { get; }
        Enum Target { get; }
        #endregion
    }
}