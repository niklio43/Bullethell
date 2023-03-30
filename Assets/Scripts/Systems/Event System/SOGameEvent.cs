using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BulletHell.GameEventSystem
{
    [CreateAssetMenu(fileName = "GameEvent", menuName = "New Game Event")]
    public class SOGameEvent : ScriptableObject
    {
        [HideInInspector] public GameEvent gameEvent;

        public void Raise(Component sender, object data)
        {
            gameEvent.Raise(sender, data);
        }

        private void OnEnable()
        {
            if(gameEvent != null)
                gameEvent.Id = name;
        }
    }
}
