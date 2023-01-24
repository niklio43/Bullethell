using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BulletHell.Abilities;

public class TestAbility : MonoBehaviour
{
    public Ability test;


    private void Start()
    {
        test = Instantiate(test);
        test.Initialize(gameObject);
    }

    private void Update()
    {
        test.UpdateAbility(Time.deltaTime);
    }

    [ContextMenu("Test")]
    public void CastAbility()
    {
        test.Cast(CastDelegateFunction);
    }

    public void CastDelegateFunction()
    {
        Debug.Log("Finished Casting");
    }
}
