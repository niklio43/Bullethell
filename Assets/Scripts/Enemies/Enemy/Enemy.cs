using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BulletHell.Enemies;
using BulletHell.Enemies.Detection;

public class Enemy : MonoBehaviour
{
    public EnemyMovmentType MovementType;
    public EnemyBrain Brain;
    public enum EnemyStates
    {
        Idle,
        Chasing,
        Attacking
    }

    private void Awake()
    {
        Brain = Instantiate(Brain);

        Brain.Initialize(this);
    }

    private void Update()
    {
        Brain.Think();
    }
}

public enum EnemyMovmentType
{
    Grounded,
    Airborne
}
