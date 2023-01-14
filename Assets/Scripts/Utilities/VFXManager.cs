using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

namespace BulletHell.VFX
{
    public class VFXManager : Singleton<VFXManager>
    {
        public static ObjectPool<RuntimeVisualEffect> Pool {
            get {
                if(_pool == null)
                    _pool = new ObjectPool<RuntimeVisualEffect>(Instance.Create, 100, Instance.name);
                return _pool;
            }
            
        }
        static ObjectPool<RuntimeVisualEffect> _pool;

        public static void PlayBurst(VisualEffectAsset asset, Vector3 position, Transform parent = null)
        {
            RuntimeVisualEffect vfx = Pool.Get();
            vfx.gameObject.SetActive(true);
            if (parent != null)
                vfx.transform.parent = parent;

            vfx.transform.localPosition = position;
            vfx.PlayBurst(asset);
        }
        public static void Play(VisualEffectAsset asset, float time, Vector3 position, Transform parent = null)
        {
            RuntimeVisualEffect vfx = Pool.Get();
            vfx.gameObject.SetActive(true);
            if (parent != null)
                vfx.transform.parent = parent;

            vfx.transform.localPosition = position;
            vfx.Play(asset, time);
        }

        RuntimeVisualEffect Create()
        {
            RuntimeVisualEffect vfx = new GameObject().AddComponent<RuntimeVisualEffect>();
            vfx.Initialize(Pool);

            return vfx;
        }
    }

    public class RuntimeVisualEffect : MonoBehaviour, IPoolable
    {
        Pool<RuntimeVisualEffect> _pool;
        VisualEffect _vfx;

        public void Initialize(Pool<RuntimeVisualEffect> pool)
        {
            _pool = pool;
            _vfx = gameObject.AddComponent<VisualEffect>();
        }

        public void PlayBurst(VisualEffectAsset asset)
        {
            _vfx.visualEffectAsset = asset;
            _vfx.Play();
            MonoInstance.Instance.StartCoroutine(PlayForSeconds(1));
        }

        public void Play(VisualEffectAsset asset, float time)
        {
            _vfx.visualEffectAsset = asset;
            _vfx.Play();
            MonoInstance.Instance.StartCoroutine(PlayForSeconds(time));
        }

        public void ResetObject()
        {
            _vfx.Stop();
            _vfx.visualEffectAsset = null;
            transform.position = Vector3.zero;
            gameObject.SetActive(false);
            _pool.Release(this);
        }

        #region Coroutines
        IEnumerator PlayForSeconds(float time)
        {
            yield return new WaitForSeconds(time);
            ResetObject();
        }

        IEnumerator CheckIfPlaying()
        {
            while (_vfx.aliveParticleCount > 0) { yield return new WaitForFixedUpdate(); }
            ResetObject();
        }
        #endregion
    }

}
