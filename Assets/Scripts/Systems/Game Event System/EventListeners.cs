using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BulletHell.GameEventSystem
{
    public class EventListeners : MonoBehaviour
    {
        [SerializeField] GameEventListener[] _listeners;

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
    }
}