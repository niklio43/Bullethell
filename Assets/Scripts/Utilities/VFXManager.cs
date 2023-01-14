using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

namespace BulletHell.VFX
{
    public class VFXManager : Singleton<VFXManager>
    {
        public static ObjectPool<RuntimeVisualEffect> _pool { get; private set; }

        protected override void OnAwake()
        {
            _pool = new ObjectPool<RuntimeVisualEffect>(Create, 100, name);
        }

        public static void PlayBurst(VisualEffectAsset asset, Vector3 position, Transform parent = null)
        {
            RuntimeVisualEffect vfx = _pool.Get();
            if (parent != null)
                vfx.transform.parent = parent;

            vfx.transform.localPosition = position;
            vfx.PlayBurst(asset);
        }
        public static void Play(VisualEffectAsset asset, float time, Vector3 position, Transform parent = null)
        {
            RuntimeVisualEffect vfx = _pool.Get();
            if (parent != null)
                vfx.transform.parent = parent;

            vfx.transform.localPosition = position;
            vfx.Play(asset, time);
        }

        RuntimeVisualEffect Create()
        {
            RuntimeVisualEffect vfx = new RuntimeVisualEffect();
            vfx.Pool = _pool;

            return vfx;
        }
    }

    public class RuntimeVisualEffect : VisualEffect, IPoolable
    {
        public Pool<RuntimeVisualEffect> Pool;
        public new string name = "(Runtime) VisualEffect";

        public void PlayBurst(VisualEffectAsset asset)
        {
            PlayBurst(asset);
            MonoInstance.Instance.StartCoroutine(CheckIfPlaying());
        }

        public void Play(VisualEffectAsset asset, float time)
        {
            visualEffectAsset = asset;
            Play();
            MonoInstance.Instance.StartCoroutine(PlayForSeconds(time));
        }

        public void ResetObject()
        {
            Stop();
            visualEffectAsset = null;
            transform.position = Vector3.zero;
            gameObject.SetActive(false);
            Pool.Release(this);
        }

        #region Coroutines
        IEnumerator PlayForSeconds(float time)
        {
            yield return new WaitForSeconds(time);
            ResetObject();
        }

        IEnumerator CheckIfPlaying()
        {
            while (aliveParticleCount > 0) { yield return new WaitForFixedUpdate(); }
            ResetObject();
        }
        #endregion
    }

}
