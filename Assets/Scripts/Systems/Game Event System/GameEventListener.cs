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
        public GameEvent Event;

        public CustomEvent Response;
        public void Initialize()
        {
            Event.Register(this);
        }

        public void UnInitialize()
        {
            Event.UnRegister(this);
        }

        public void OnEventRaised(Component sender, object data)
        {
            Response?.Invoke(sender, data);
        }
    }
}
