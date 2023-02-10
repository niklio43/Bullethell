using BulletHell;
using BulletHell.Enemies;
using BulletHell.Enemies.Detection;
using BulletHell.Stats;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Bullet.CameraUtilities;
using BulletHell.VFX;
using UnityEngine.VFX;

public class Enemy : MonoBehaviour
{
    public EnemyMovmentType MovementType = EnemyMovmentType.Grounded;
    [SerializeField] EnemyBrain _brain;
    [SerializeField] EnemyMovement _enemyMovement;

    [HideInInspector] public Transform Target;
    [HideInInspector] public bool CanMove = true;
    [HideInInspector] public DetectionData DetectionData = new DetectionData();
    public EnemyAim Aim;
    public EnemyWeapon Weapon;

    RuntimeVisualEffect _stunVFX = null;

    EnemyDetection _detection;
    Character _character;

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
        _character = GetComponent<Character>();
        _detection = GetComponent<EnemyDetection>();

        _enemyMovement.Initialize(this);

        _character.OnTakeDamageEvent += TakeDamage;
        _character.OnStunEvent += OnStun;
        _character.OnExitStunEvent += OnExitStun;
        _character.OnDeathEvent += OnDeath;

        _brain = Instantiate(_brain);
        _brain.Initialize(this);

        _defaultScale = transform.localScale;
    }

    private void Update()
    {
        _brain.UpdateBrain(Time.deltaTime);
        DetectionData = _detection.Detect();

        UpdateDirection();
        UpdateTarget();
        Aim.UpdateAim(Target);
        if (CanMove) _enemyMovement.Move();
    }

    public void TakeDamage(float damage)
    {
        Camera.main.Shake(0.075f, 0.1f);
        _brain.SetState(EnemyBrain.EnemyStates.Staggered);
        //DamagePopupManager.Instance.InsertIntoPool(1f, transform.position);
    }

    public void OnStun()
    {
        _brain.CurrentAbility?.Cancel();
        _brain.SetState(EnemyBrain.EnemyStates.Stunned);

        _stunVFX = BulletHell.VFX.VFXManager.PlayUntilStopped(Resources.Load<VisualEffectAsset>("StunnedEffect"), Vector3.up / 2, transform);
    }

    public void OnExitStun()
    {
        _brain.SetState(EnemyBrain.EnemyStates.Idle);

        _stunVFX.Stop();
    }

    public void OnDeath()
    {
        _brain.CurrentAbility?.Cancel();
        Destroy(gameObject);
    }

    public void UpdateDirection()
    {
        if (Target == null) { return; }
        Vector2 targetDirection = Target.position - transform.position;

        if (Vector2.Dot(targetDirection, Vector2.right) < 0 && !_flipped)
        {
            transform.localScale = new Vector3(_defaultScale.x * -1, _defaultScale.y, _defaultScale.z);
            _flipped = true;
            Aim.Flip();
        }
        else if (Vector2.Dot(targetDirection, Vector2.right) > 0 && _flipped)
        {
            transform.localScale = _defaultScale;
            _flipped = false;
            Aim.Flip();
        }
    }

    public void UpdateTarget()
    {
        if (DetectionData["Players"].Length > 0)
        {
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
        if (_cachedComponents.ContainsKey(typeof(T)))
        {
            return (T)_cachedComponents[typeof(T)];
        }

        var component = base.GetComponent<T>();
        if (component != null)
        {
            _cachedComponents.Add(typeof(T), component);
        }
        return component;
    }

    #endregion
}
