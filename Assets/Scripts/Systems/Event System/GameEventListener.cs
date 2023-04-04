using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace BulletHell.GameEventSystem
{
    [System.Serializable]
    public class CustomEvent : UnityEvent<Component, object> {}

    [System.Serializable]
    public class GameEventListener
    {
        #region Public Fields
        public SOGameEvent Event;
        #endregion

        #region Private Fields
        [SerializeField] CustomEvent _response;
        #endregion

        #region Public Fields
        public void Initialize()
        {
            GameEventManager.GetEvent(Event.name).Register(this);
        }

        public void UnInitialize()
        {
            GameEventManager.GetEvent(Event.name).UnRegister(this);
        }

        public void OnEventRaised(Component sender, object data)
        {
            _response?.Invoke(sender, data);
        }
        #endregion
    }
}
