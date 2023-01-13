using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BulletHell.Emitters;

namespace BulletHell.Abilities.New
{
    [CreateAssetMenu(fileName = "EmitterAbilityBehaviour", menuName = "Abilities/New Emitter Ability Behaviour")]
    public class EmitterAbilityBehaviour : BaseAbilityBehaviour
    {
        [SerializeField] List<EmitterData> _emitters;
        List<Emitter> _emitterObjects;

        public override void Initialize(Ability ability, GameObject owner)
        {
            foreach (EmitterData emitterData in _emitters) {
                Emitter emitter = new GameObject($"{ability.GetName()} (Emitter)").AddComponent<Emitter>();
                emitter.transform.SetParent(owner.transform);
                emitter.transform.localPosition = Vector3.zero;
                emitter.Data = emitterData;
                emitter.AutoFire = false;

                _emitterObjects.Add(emitter);
            }
        }
        public override void Unitialize()
        {
            foreach (Emitter emitter in _emitterObjects) {
                MonoBehaviour.Destroy(emitter.gameObject);
            }
        }

        public override void Perform(GameObject owner)
        {
            foreach (Emitter emitter in _emitterObjects) {
                emitter.FireProjectile();
            }
        }
    }
}
