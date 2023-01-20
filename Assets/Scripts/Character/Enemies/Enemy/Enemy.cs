using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BulletHell.Enemies;
using BulletHell.Enemies.Detection;
using System;
using System.Linq;
using BulletHell;
using BulletHell.Stats;

public class Enemy : Character
{
    public EnemyMovmentType MovementType = EnemyMovmentType.Grounded;
    [HideInInspector] public Transform Target;
    [HideInInspector] public bool CanMove = true;
    [HideInInspector] public Animator Animator { get; private set; }
    [HideInInspector] public DetectionData DetectionData = new DetectionData();
    public EnemyAim Aim;

    [SerializeField] EnemyBrain _brain;
    EnemyDetection _detection;
    EnemyMovement _enemyMovement;

    public enum EnemyMovmentType
    {
        Grounded,
        Airborne
    }

    [Header("Distances")]
    [Range(0, 10)] public float PreferredDistance;
    [Range(0, 10)] public float AttackDistance;

    private void Awake()
    {
        Animator = GetComponentInChildren<Animator>();
        _detection = GetComponent<EnemyDetection>();
        _enemyMovement = GetComponent<EnemyMovement>();

        Initialize();

        _brain = Instantiate(_brain);
        _brain.Initialize(this);
    }

    private void Update()
    {
        if (Target != null) {
            Vector2 targetDirection = Target.position - transform.position;
            if ((Vector2.Dot(targetDirection, Vector2.right) < 0)) {
                transform.localScale = new Vector3(-1, 1, 1);
            }else {
                transform.localScale = new Vector3(1, 1, 1);
            }
        }

        DetectionData = _detection.Detect();
        _brain.Think();

        if (DetectionData["Players"].Length > 0) {
            Transform target = DetectionData["Players"].OrderBy(n => Vector2.Distance(transform.position, n.transform.position)).First().transform;
            SetTarget(target);
        }

        Aim.UpdateAim(Target);
        if(CanMove) _enemyMovement.Move();
    }

    public override void TakeDamage(DamageInfo damage)
    {
        base.TakeDamage(damage);

        DamagePopupManager.Instance.InsertIntoPool(1f, transform.position);
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

    public bool TargetTooClose()
    {
        float distance = Vector2.Distance(transform.position, Target.position);

        return (distance < PreferredDistance);
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
