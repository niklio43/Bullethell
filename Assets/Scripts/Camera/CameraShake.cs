using System.Collections;
using UnityEngine;

public class CameraShake : Singleton<CameraShake>
{
    public static void Shake(float duration, float amount)
    {
        Instance.StopAllCoroutines();
        Instance.StartCoroutine(cShake(duration, amount));
    }

    static IEnumerator cShake(float duration, float amount)
    {
        Vector3 startPosition = Instance.transform.position;

        float timeElapsed = 0;

        while(timeElapsed < duration)
        {
            yield return new WaitForEndOfFrame();
            timeElapsed += Time.deltaTime;

            Instance.transform.position = startPosition + (Vector3)Random.insideUnitCircle * amount;
        }

        Instance.transform.localPosition = startPosition;
    }
}