using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BulletHell.Enemies.Steering;


namespace BulletHell.Enemies
{
    [CreateAssetMenu(fileName = "EnemyStats", menuName = "Enemies/Stats")]
    public class EnemyStats : CharacterStats
    {
        public enum EnemyMovementType
        {
            Grounded,
            Flying
        }

        public EnemyMovementType MovementType;

        [Range(0, 10)] public float AttackDistance;
        [Range(0, 10)] public float PreferredDistance;
    }
}