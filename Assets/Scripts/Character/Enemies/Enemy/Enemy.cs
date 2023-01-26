using BulletHell;
using BulletHell.Enemies;
using BulletHell.Enemies.Detection;
using BulletHell.Stats;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Enemy : Character
{
    public EnemyMovmentType MovementType = EnemyMovmentType.Grounded;
    [HideInInspector] public Transform Target;
    [HideInInspector] public bool CanMove = true;
    [HideInInspector] public DetectionData DetectionData = new DetectionData();
    public EnemyAim Aim;
    public EnemyWeapon Weapon;

    [SerializeField] EnemyBrain _brain;

    EnemyDetection _detection;
    EnemyMovement _enemyMovement;

    public bool Invincible { get; set; } = false;
    bool _flipped = false;
    Vector3 _defaultScale;

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
        _detection = GetComponent<EnemyDetection>();
        _enemyMovement = GetComponent<EnemyMovement>();

        Initialize();

        _brain = Instantiate(_brain);
        _brain.Initialize(this);

        _defaultScale = transform.localScale;
    }

    private void Update()
    {
        _brain.Update();
        DetectionData = _detection.Detect();

        UpdateDirection();
        UpdateTarget();
        Aim.UpdateAim(Target);
        if (CanMove) _enemyMovement.Move();
    }

    public override void TakeDamage(DamageInfo damage)
    {
        if(Invincible) { return; }

        Stats["Hp"].Value -= DamageCalculator.MitigateDamage(damage, Stats);

        Camera.main.Shake(0.1f, 0.3f);

        if (Stats["Hp"].Value <= 0) {
            OnDeath();
        }

        _brain.SetState(EnemyBrain.EnemyStates.Staggered);


        //DamagePopupManager.Instance.InsertIntoPool(1f, transform.position);
    }

    public void UpdateDirection()
    {
        if (Target == null) { return; }
        Vector2 targetDirection = Target.position - transform.position;

        if(Vector2.Dot(targetDirection, Vector2.right) < 0 && !_flipped) {
            transform.localScale = new Vector3(_defaultScale.x * -1, _defaultScale.y, _defaultScale.z);
            _flipped = true;
            Aim.Flip();
        }
        else if(Vector2.Dot(targetDirection, Vector2.right) > 0 && _flipped) {
            transform.localScale = _defaultScale;
            _flipped = false;
            Aim.Flip();
        }
    }

    public void UpdateTarget()
    {
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
