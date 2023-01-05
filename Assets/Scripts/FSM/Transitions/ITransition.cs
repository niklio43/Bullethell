using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BulletHell.FiniteStateMachine
{
    public interface ITransition 
    {
        string Name { get; }
        Enum Target { get; }
    }
}