using BulletHell;
using BulletHell.Enemies;
using BulletHell.Enemies.Detection;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using BulletHell.CameraUtilities;
using BulletHell.VFX;
using UnityEngine.VFX;
using BulletHell.EffectInterfaces;

public class Enemy : MonoBehaviour, IKillable, IStaggerable, IStunable
{
    #region Private Fields
    [SerializeField] EnemyBrain _brain;
    [SerializeField] EnemyMovement _enemyMovement;
    [SerializeField] VisualEffectAsset _deathVFX;
    [SerializeField] DropTable _dropTable;
    RuntimeVisualEffect _stunVFX = null;

    EnemyDetection _detection;

    bool _flipped = false;
    Vector3 _defaultScale;
    #endregion

    #region Public Fields
    public EnemyMovmentType MovementType = EnemyMovmentType.Grounded;
    [HideInInspector] public Transform Target;
    [HideInInspector] public bool CanMove = true;
    [HideInInspector] public DetectionData DetectionData = new DetectionData();
    public EnemyAim Aim;
    public EnemyWeapon Weapon;

    public delegate void OnDeathDelegate(Enemy enemy);
    public event OnDeathDelegate OnDeath;
    public enum EnemyMovmentType
    {
        Grounded,
        Airborne
    }
    [Header("Distances")]
    [Range(0, 10)] public float PreferredDistance;
    #endregion

    #region Private Methods
    private void Awake()
    {
        _detection = GetComponent<EnemyDetection>();

        _enemyMovement.Initialize(this);
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

    private void OnDestroy()
    {

    }
    #endregion

    #region Public Methods
    public void Stun()
    {
        _brain.CurrentAbility?.Cancel();
        _brain.SetState(EnemyBrain.EnemyStates.Stunned);
        _brain.LockState = true;
        _stunVFX = BulletHell.VFX.VFXManager.PlayUntilStopped(Resources.Load<VisualEffectAsset>("StunnedEffect"), Vector3.up / 2, transform);
    }

    public void ExitStun()
    {
        _brain.LockState = false;
        _brain.SetState(EnemyBrain.EnemyStates.Idle);
        _stunVFX.Stop();
    }

    public void Stagger()
    {
        _brain.SetState(EnemyBrain.EnemyStates.Staggered);
    }

    [ContextMenu("Kill")]
    public void Kill()
    {
        _brain.CurrentAbility?.Cancel();
        BulletHell.VFX.VFXManager.Play(_deathVFX, 1, transform.position);
        DropRandomLoot.Instance?.DropItem(_dropTable.ItemDrop, transform);
        OnDeath?.Invoke(this);
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

    public bool TargetTooClose()
    {
        float distance = Vector2.Distance(transform.position, Target.position);

        return (distance < PreferredDistance);
    }

    public bool TargetInLineOfSight(Vector3 origin)
    {
        Vector2 direction = Target.position - transform.position;
        float distance = Vector2.Distance(transform.position, Target.position);
        LayerMask mask = 1 << LayerMask.NameToLayer("Obstacle");

        RaycastHit2D hit = Physics2D.Raycast(origin, direction, distance, mask);

        if (hit.collider == null) return true;
        return false;

    }
    #endregion

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
