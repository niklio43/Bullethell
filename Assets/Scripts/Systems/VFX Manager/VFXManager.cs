using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

namespace BulletHell.VFX
{
    public class VFXManager : Singleton<VFXManager>
    {
        public static ObjectPool<RuntimeVisualEffect> Pool
        {
            get {
                if (_pool == null)
                    _pool = new ObjectPool<RuntimeVisualEffect>(Instance.Create, 100, "VFX Pool");
                return _pool;
            }

        }
        static ObjectPool<RuntimeVisualEffect> _pool;

        public static RuntimeVisualEffect PlayBurst(VisualEffectAsset asset, Vector3 position, Transform parent = null, VFXAttribute[] vfxAttributes = null)
        {
            RuntimeVisualEffect vfx = Pool.Get();
            vfx.gameObject.SetActive(true);
            if (parent != null)
                vfx.transform.parent = parent;

            vfx.transform.localPosition = position;
            vfx.PlayBurst(asset, vfxAttributes);
            return vfx;
        }
        public static RuntimeVisualEffect Play(VisualEffectAsset asset, float time, Vector3 position, Transform parent = null, VFXAttribute[] vfxAttributes = null, bool waitForParticles = false)
        {
            RuntimeVisualEffect vfx = Pool.Get();
            vfx.gameObject.SetActive(true);
            if (parent != null)
                vfx.transform.parent = parent;

            vfx.transform.localPosition = position;
            vfx.Play(asset, time, vfxAttributes, waitForParticles);
            return vfx;
        }

        public static RuntimeVisualEffect PlayUntilStopped(VisualEffectAsset asset, Vector3 position, Transform parent = null, VFXAttribute[] vfxAttributes = null, bool waitForParticles = false)
        {
            RuntimeVisualEffect vfx = Pool.Get();
            vfx.gameObject.SetActive(true);
            if (parent != null)
                vfx.transform.parent = parent;

            vfx.transform.localPosition = position;
            vfx.PlayUntilStopped(asset, vfxAttributes, waitForParticles);
            return vfx;
        }

        RuntimeVisualEffect Create()
        {
            RuntimeVisualEffect vfx = new GameObject().AddComponent<RuntimeVisualEffect>();
            vfx.name = "Runtime VisualEffect";
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
            _vfx.GetComponent<Renderer>().sortingLayerID = SortingLayer.NameToID("Top");
        }

        public void PlayBurst(VisualEffectAsset asset, VFXAttribute[] vfxAttributes = null)
        {
            _vfx.visualEffectAsset = asset;

            if (vfxAttributes != null)
                foreach (VFXAttribute attribute in vfxAttributes) {
                    attribute.SetValue(_vfx);
                }

            _vfx.Play();

            //TODO Fix so that the VFX stops playing when burst has finished.
            MonoInstance.Instance.StartCoroutine(PlayForSeconds(.5f));
        }

        public void Play(VisualEffectAsset asset, float time, VFXAttribute[] vfxAttributes = null, bool waitForParticles = false)
        {
            _vfx.visualEffectAsset = asset;

            if (vfxAttributes != null)
                foreach (VFXAttribute attribute in vfxAttributes) {
                    attribute.SetValue(_vfx);
                }

            _vfx.Play();
            MonoInstance.Instance.StartCoroutine(PlayForSeconds(time, waitForParticles));
        }

        public void PlayUntilStopped(VisualEffectAsset asset, VFXAttribute[] vfxAttributes = null, bool waitForParticles = false)
        {
            _vfx.visualEffectAsset = asset;

            if (vfxAttributes != null)
                foreach (VFXAttribute attribute in vfxAttributes) {
                    attribute.SetValue(_vfx);
                }

            _vfx.Play();
        }

        public void Stop()
        {
            _vfx?.Stop();
            ResetObject();
        }

        public void ResetObject()
        {
            _vfx.Stop();
            _vfx.visualEffectAsset = null;
            transform.position = Vector3.zero;
            gameObject.SetActive(false);
            _pool.Release(this);
        }

        IEnumerator PlayForSeconds(float time, bool waitForParticles = false)
        {
            yield return new WaitForSeconds(time);
            _vfx.Stop();

            if(waitForParticles)
                while (_vfx.aliveParticleCount > 0) {
                    yield return new WaitForFixedUpdate();
                }

            ResetObject();
        }
    }

    #region VFX Attributes
    public abstract class VFXAttribute
    {
        protected readonly string _attributeName;
        public VFXAttribute(string name)
        {
            _attributeName = name;
        }
        public abstract void SetValue(VisualEffect vfx);
    }
    public class VFXBool : VFXAttribute
    {
        private readonly bool _value;
        public VFXBool(string name, bool value) : base(name)
        {
            _value = value;
        }
        public override void SetValue(VisualEffect vfx)
        {
            vfx.SetBool(_attributeName, _value);
        }
    }
    public class VFXFloat : VFXAttribute
    {
        private readonly float _value;
        public VFXFloat(string name, float value) : base(name)
        {
            _value = value;
        }
        public override void SetValue(VisualEffect vfx)
        {
            vfx.SetFloat(_attributeName, _value);
        }
    }
    public class VFXInt : VFXAttribute
    {
        private readonly int _value;
        public VFXInt(string name, int value) : base(name)
        {
            _value = value;
        }
        public override void SetValue(VisualEffect vfx)
        {
            vfx.SetInt(_attributeName, _value);
        }
    }
    #endregion
}
