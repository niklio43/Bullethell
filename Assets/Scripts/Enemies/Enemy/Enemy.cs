using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BulletHell.Enemies;
using BulletHell.Enemies.Detection;
using System;
using System.Linq;

public class Enemy : MonoBehaviour
{
    public EnemyStats Stats;
    
    [SerializeField] public EnemyBrain _brain;
    EnemyDetection _detection;

    public Transform Target;

    public DetectionData DetectionData = new DetectionData();


    private void Awake()
    {
        _brain = Instantiate(_brain);
        _brain.Initialize(this);
        
        _detection = GetComponent<EnemyDetection>();
    }

    private void Update()
    {
        DetectionData = _detection.Detect();
        _brain.Think(DetectionData);

        if (DetectionData["Players"].Length > 0) {
            Transform target = DetectionData["Players"].OrderBy(n => Vector2.Distance(transform.position, n.transform.position)).First().transform;
            SetTarget(target);
        }
    }

    public void SetTarget(Transform target)
    {
        Target = target;
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
