using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BulletHell.CameraUtilities
{
    public static class CameraExtensions
    {
        static Coroutine CurrentShakeRoutine;
        static Coroutine CurrentZoomRoutine;

        public static void Shake(this Camera camera, float duration, float amount)
        {
            if (CurrentShakeRoutine != null) { return; }
            CurrentShakeRoutine = MonoInstance.Instance.StartCoroutine(cShake(camera, duration, amount));
        }

        public static void Zoom(this Camera camera, float duration, float amount)
        {
            if(CurrentZoomRoutine != null) { return; }
            CurrentZoomRoutine = MonoInstance.Instance.StartCoroutine(ZoomInOut(camera, duration, amount));
        }

        static IEnumerator ZoomInOut(Camera camera, float duration, float amount)
        {
            float startZoom = camera.orthographicSize;

            yield return cZoom(camera, duration / 2, startZoom, startZoom + amount);
            yield return cZoom(camera, duration / 2, startZoom + amount, startZoom);

            camera.orthographicSize = startZoom;
            CurrentZoomRoutine = null;
        }

        static IEnumerator cZoom(Camera camera, float duration, float from, float to)
        {
            float timeElapsed = 0;

            while(timeElapsed < duration) {
                yield return new WaitForEndOfFrame();
                timeElapsed += Time.deltaTime;

                camera.orthographicSize = Mathf.Lerp(from, to, timeElapsed / duration);
            }

            camera.orthographicSize = to;
        }


        static IEnumerator cShake(Camera camera, float duration, float amount)
        {
            Vector3 startPosition = camera.transform.position;

            float timeElapsed = 0;

            while (timeElapsed < duration) {
                yield return new WaitForEndOfFrame();
                timeElapsed += Time.deltaTime;

                camera.transform.position = startPosition + (Vector3)UnityEngine.Random.insideUnitCircle * amount;
            }

            camera.transform.position = startPosition;
            CurrentShakeRoutine = null;
        }
    }
}
