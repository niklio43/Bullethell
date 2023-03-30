using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BulletHell.GameEventSystem
{
    [System.Serializable]
    public class GameEvent
    {
        #region Public Fields
        public string Id;
        #endregion

        #region Private Fields
        private List<GameEventListener> _eventListeners = new List<GameEventListener>();
        private Action<Component, object> _onEventRaised;
        #endregion

        #region Public Methods
        public GameEvent(string id)
        {
            Id = id;
        }

        public void Raise(Component sender, object data)
        {
            for (int i = 0; i < _eventListeners.Count; i++) {
                _eventListeners[i].OnEventRaised(sender, data);
            }

            _onEventRaised?.Invoke(sender, data);
        }

        public void Register(GameEventListener listener)
        {
            if (!_eventListeners.Contains(listener))
                _eventListeners.Add(listener);
        }

        public void RegisterCallBack(Action<Component, object> Callback)
        {
            _onEventRaised += Callback;
        }

        public void UnRegister(GameEventListener listener)
        {
            if (_eventListeners.Contains(listener))
                _eventListeners.Remove(listener);
        }

        public void UnRegisterCallback(Action<Component, object> Callback)
        {
            _onEventRaised -= Callback;
        }
        #endregion
    }
}
