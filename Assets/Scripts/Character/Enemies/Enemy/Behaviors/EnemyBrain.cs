using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BulletHell.FiniteStateMachine;
using BulletHell.Enemies.Detection;

namespace BulletHell.Enemies
{
    public abstract class EnemyBrain : ScriptableObject
    {
        public IFSM FSM;
        public abstract void Initialize(Enemy enemy);

        public virtual void Think()
        {
            FSM.Update();
        }

    }
}