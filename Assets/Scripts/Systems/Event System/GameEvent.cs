using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BulletHell.GameEventSystem
{
    [CreateAssetMenu(fileName = "GameEvent", menuName = "New GameEvent", order = 0)]
    public class GameEvent : ScriptableObject
    {
        #region Private Fields
        private List<GameEventListener> _eventListeners = new List<GameEventListener>();
        #endregion

        #region Public Methods
        public void Raise(Component sender, object data)
        {
            for (int i = 0; i < _eventListeners.Count; i++) {
                _eventListeners[i].OnEventRaised(sender, data);
            }
        }
        public void Register(GameEventListener listener)
        {
            if (!_eventListeners.Contains(listener))
                _eventListeners.Add(listener);
        }

        public void UnRegister(GameEventListener listener)
        {
            if(_eventListeners.Contains(listener))
                _eventListeners.Remove(listener);
        }
        #endregion
    }
}
