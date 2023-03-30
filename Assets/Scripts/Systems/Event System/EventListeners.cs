using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BulletHell.GameEventSystem
{
    public class EventListeners : MonoBehaviour
    {
        #region Private Fields
        [SerializeField] GameEventListener[] _listeners;
        #endregion

        #region Private Methods
        private void OnEnable()
        {
            for (int i = 0; i < _listeners.Length; i++) {
                _listeners[i].Initialize();
            }
        }

        private void OnDisable()
        {
            for (int i = 0; i < _listeners.Length; i++) {
                _listeners[i].UnInitialize();
            }
        }
        #endregion
    }
}