using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static BulletHell.Utilities;

namespace BulletHell.Emitters
{
    public class EmitterGroups
    {
        public EmitterGroup this[int i] => _emitterGroups[i];

        List<EmitterGroup> _emitterGroups;

        Transform _transform;

        public EmitterGroups(Transform transform)
        {
            _transform = transform;
            _emitterGroups = new List<EmitterGroup>();
        }

        public void UpdateGroups(EmitterData emitterData, List<EmitterModifier> modifiers)
        {
            CreateGroups(emitterData.EmitterPoints);
            RefreshGroups(emitterData, modifiers);
        }

        public void CreateGroups(int amount)
        {
            if(_emitterGroups.Count == amount) { return; }
            if(amount < _emitterGroups.Count) { _emitterGroups.RemoveRange(amount, _emitterGroups.Count - amount); return; }

            int amountToCreate = amount - _emitterGroups.Count;
            for (int i = 0; i < amountToCreate; i++) {
                _emitterGroups.Add(new EmitterGroup());
            }
        }
        public void RefreshGroups(EmitterData emitterData, List<EmitterModifier> modifiers)
        {
            for (int n = 0; n < emitterData.EmitterPoints; n++) {

                _emitterGroups[n].ClearModifier();

                float spread = n * emitterData.Spread;
                float pitch = emitterData.Pitch;
                float offset = emitterData.Offset;


                EmitterModifier activeModifier = null;

                for (int i = 0; i < modifiers.Count; i++) {
                    if(!modifiers[i].Enabled) { continue; }
                    int value = ((n + 1) % modifiers[i].Factor) - modifiers[i].Count;
                    if (value > 0) { continue; }

                    activeModifier = modifiers[i];
                    spread = n * emitterData.Spread + modifiers[i].Spread;
                    pitch = modifiers[i].Pitch;
                    offset = modifiers[i].Offset;
                }

                float centerSpread = (spread * ((emitterData.EmitterPoints - 1) / 2f));
                float rotation = spread + emitterData.CenterRotation + emitterData.ParentRotation;
                Vector2 positon = (Rotate(emitterData.Direction, rotation).normalized * offset) + (Vector2)_transform.position;

                Vector2 direction = Rotate(emitterData.Direction, rotation + pitch).normalized;

                _emitterGroups[n].Set(positon, direction);
                _emitterGroups[n].SetModifier(activeModifier);
            }
        }
    }
}
