using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BulletHell.Enemies;
using BulletHell.Enemies.Detection;
using System;
using System.Linq;

public class Enemy : Character
{
    public Transform Target;
    public DetectionData DetectionData = new DetectionData();
    public EnemyMovmentType MovementType = EnemyMovmentType.Grounded;
    [SerializeField] public EnemyBrain _brain;
    public bool CanMove = false;

    EnemyDetection _detection;
    public EnemyMovement _enemyMovement;


    [SerializeField] Transform _damagePopupPrefab;

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
        _enemyMovement = GetComponent<EnemyMovement>();
    }

    private void Update()
    {
        if (Target != null) {
            Vector2 targetDirection = Target.position - transform.position;
            _spriteRenderer.flipX = (Vector2.Dot(targetDirection, Vector2.right) < 0) ? true : false;
        }

        DetectionData = _detection.Detect();
        _brain.Think();

        if (DetectionData["Players"].Length > 0) {
            Transform target = DetectionData["Players"].OrderBy(n => Vector2.Distance(transform.position, n.transform.position)).First().transform;
            SetTarget(target);
        }

        if(CanMove) 
            _enemyMovement.Move();

        _animator.SetBool("Walking", (GetComponent<Rigidbody2D>().velocity.SqrMagnitude() > 1));
    }

    public override void TakeDamage(float amount)
    {
        base.TakeDamage(amount);

        DamagePopup damagePopup = Instantiate(_damagePopupPrefab, transform.position, Quaternion.identity).GetComponent<DamagePopup>();
        damagePopup.Setup(amount);
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