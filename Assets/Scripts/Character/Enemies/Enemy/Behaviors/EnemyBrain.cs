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

        public enum EnemyStates
        {
            Idle,
            Chasing,
            Attacking
        }

        public abstract void Initialize(Enemy enemy);

        public void Think()
        {
            FSM.Update();
        }

    }
}