using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BulletHell.Enemies;
using BulletHell.Enemies.Detection;
using System;

public class Enemy : MonoBehaviour
{
    public EnemyStats Stats;
    public EnemyBrain Brain;
    public EnemyDetection Detection;
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

        Detection = new EnemyDetection(this);
    }

    private void Update()
    {
        Detection.Detect();
        Brain.Think();
    }

    Dictionary<Type, Component> _cachedComponents = new Dictionary<Type, Component>();
    public new T GetComponent<T>() where T : Component
    {
        if (_cachedComponents.ContainsKey(typeof(T))) {
            return (T)_cachedComponents[typeof(T)];
        }

        var component = base.GetComponent<T>();
        if (component != null) {
            _cachedComponents.Add(typeof(T), component);
        }
        return component;
    }
}
