using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirborneProjectile : MonoBehaviour
{
    [SerializeField, Range(1, 20)] float gravity = 9.8f;
    [SerializeField, Range(0, 10)] float launchHeight = 1.0f;
    [SerializeField, Range(1, 10)] float MaxHeight = 8;
    [SerializeField] float _speed = 1;

    [SerializeField] Vector2 _target;

    float startSize;


    [ContextMenu("Test")]
    public void Test()
    {
        Fire(_target);
    }

    public void Fire(Vector2 target)
    {
        startSize = transform.localScale.x;
        transform.position = new Vector2(transform.position.x, transform.position.y + launchHeight);
        StartCoroutine(ArcRoutine(target));
    }

    IEnumerator ArcRoutine(Vector2 target)
    {
        Vector2 dir = target - (Vector2)transform.position;
        Vector2 normalizedDir = dir.normalized;
        
        float dist = Vector2.Distance(transform.position, target);
        float duration = dist / _speed;

        DamageZone zone = DamageZoneManager.PlaceZone(target);
        zone.Execute(2);

        float initalVerticalVelocity = duration * (gravity / 2) - launchHeight / duration + launchHeight;
        Vector3 velocity = new Vector3(normalizedDir.x * _speed, normalizedDir.y * _speed, initalVerticalVelocity);

        float timeElapsed = 0;

        while (timeElapsed < duration) {
            yield return new WaitForFixedUpdate();
            timeElapsed += Time.fixedDeltaTime;
            if (timeElapsed >= duration) break;


            velocity.z -= gravity * Time.fixedDeltaTime;
            AddVelocity(velocity);
        }

        zone.Activate();
        transform.position = target;
    }

    void AddVelocity(Vector3 velocity)
    {
        Vector2 scaledVelocity = new Vector2(velocity.x * Time.fixedDeltaTime, velocity.y * Time.fixedDeltaTime + velocity.z * Time.fixedDeltaTime);
        transform.position += (Vector3)scaledVelocity;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(_target, Vector3.one);
    }

}
