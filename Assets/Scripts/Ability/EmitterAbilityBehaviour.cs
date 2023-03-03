using BulletHell.Emitters;
using UnityEngine;

namespace BulletHell.Abilities
{
    [CreateAssetMenu(fileName = "EmitterAbilityBehaviour", menuName = "Abilities/New Emitter Ability Behaviour")]
    public class EmitterAbilityBehaviour : BaseAbilityBehaviour
    {
        [SerializeField] protected EmitterData _emitterData;
        protected Emitter _emitterObject;
        protected override void Initialize()
        {
            _emitterObject = new Emitter(_ability.Host, _emitterData);
        }

        protected override void OnUpdate(float dt)
        {
            _emitterObject.UpdateEmitter();
        }

        public override void Uninitialize()
        {
            if (_emitterObject == null) return;
            _emitterObject = null;
        }

        protected override void Perform()
        {
            _emitterObject.FireProjectile(Target, _ability.Owner);
        }
    }
}
