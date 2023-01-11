using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BulletHell.Enemies;
using BulletHell.Enemies.Detection;
using System;
using System.Linq;

public class Enemy : Character
{
    [SerializeField] public EnemyBrain _brain;
    EnemyDetection _detection;

    public Transform Target;

    public DetectionData DetectionData = new DetectionData();

    public EnemyMovmentType MovementType = EnemyMovmentType.Grounded;

    public enum EnemyMovmentType
    {
        Grounded,
        Airborne
    }

    [Range(0, 10)] public float PreferredDistance;
    [Range(0, 10)] public float AttackDistance;

    private void Awake()
    {
        Initialize();
        _brain = Instantiate(_brain);
        _brain.Initialize(this);
        
        _detection = GetComponent<EnemyDetection>();
    }

    private void Update()
    {
        if (Target != null) {
            Vector2 targetDirection = Target.position - transform.position;
            GetComponent<SpriteRenderer>().flipX = (Vector2.Dot(targetDirection, Vector2.right) < 0) ? true : false;
        }

        DetectionData = _detection.Detect();
        _brain.Think();

        if (DetectionData["Players"].Length > 0) {
            Transform target = DetectionData["Players"].OrderBy(n => Vector2.Distance(transform.position, n.transform.position)).First().transform;
            SetTarget(target);
        }
    }

    public void SetTarget(Transform target)
    {
        Target = target;
    }

    public bool TargetInAttackRange()
    {
        float distance = Vector2.Distance(transform.position, Target.position);

        return (distance < AttackDistance);
    }


    #region Component Caching

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

    #endregion
}
