using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BulletHell.Map.RoomEvents
{
    [System.Serializable]
    public abstract class RoomEvent
    {
        #region Public Fields
        public bool ShowGizmos = false;
        public string Name = "RoomEvent";
        public StartPolicy StartPolicy;
        public float Delay;
        #endregion

        #region Private Fields
        protected Room _room;
        protected RoomEventQueue _queue;
        
        Action _onStart;
        Action _onFinished;
        #endregion

        #region Public Methods
        public RoomEvent(RoomEventQueue queue, string name)
        {
            Name = name;
            _queue = queue;
        }

        public void StartEvent(Room room)
        {
            _room = room;
            _room.StartCoroutine(StartDelay());
        }
        public void OnStart(Action evt) => _onStart += evt;
        public void OnFinished(Action evt) => _onFinished += evt;
        #endregion

        #region Private Methods
        protected abstract void StartEvent();
        protected virtual void FinishedEvent() { }

        protected void Done()
        {
            FinishedEvent();
            _onFinished?.Invoke();
            _onFinished = null;
        }

        IEnumerator StartDelay()
        {
            yield return new WaitForSeconds(Delay);
            StartEvent();
            _onStart?.Invoke();
            _onStart = null;
        }
        #endregion

        #region Gizmos
        public virtual void DrawGizmosSelected(Transform transform) { }
        public virtual void DrawGizmos(Transform transform) { }
        #endregion
    }

    public enum StartPolicy
    {
        With,
        After
    }
}
