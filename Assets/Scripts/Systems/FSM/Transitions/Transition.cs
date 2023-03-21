using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BulletHell.FiniteStateMachine
{
    public class Transition : ITransition
    {
        public string Name { get; }

        public Enum Target { get; }

        public Transition(string name, Enum target)
        {
            Name = name;
            Target = target;
        }
    }
}
