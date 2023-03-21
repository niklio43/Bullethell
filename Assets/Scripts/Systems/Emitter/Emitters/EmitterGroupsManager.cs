using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static BulletHell.Utilities;

namespace BulletHell.Emitters
{
    public class EmitterGroupsManager
    {
        public EmitterGroup this[int i] => _emitterGroups[i];

        List<EmitterGroup> _emitterGroups;
        EmitterData _emitterData;
        Transform _transform;

        public EmitterGroupsManager(Transform transform, EmitterData emitterData)
        {
            _emitterData = emitterData;
            _transform = transform;
            _emitterGroups = new List<EmitterGroup>();
        }

        public void UpdateGroups()
        {
            CreateGroups(_emitterData.EmitterPoints);
            RefreshGroups();
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
        public void RefreshGroups()
        {
            for (int n = 0; n < _emitterData.EmitterPoints; n++) {

                float spread = n * _emitterData.Spread;
                float pitch = _emitterData.Pitch;
                float offset = _emitterData.Offset;
                //float centerSpread = (Mathf.CeilToInt((emitterData.EmitterPoints - 1) / 2f)) * emitterData.Spread;

                float rotation = spread + _emitterData.CenterRotation + _emitterData.ParentRotation;
                Vector2 positon = (Rotate(_emitterData.Direction, rotation).normalized * offset) + (Vector2)_transform.position;
                Vector2 direction = Rotate(_transform.up, rotation + pitch).normalized;

                _emitterGroups[n].Set(positon, direction);
            }
        }
    }
}
