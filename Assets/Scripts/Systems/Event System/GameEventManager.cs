using System.Collections.Generic;
using UnityEngine;

namespace BulletHell.GameEventSystem
{
    public class GameEventManager
    {
        #region Private Fields
        private static Dictionary<string, GameEvent> _gameEvents;
        #endregion

        #region Public Methods
        public static GameEvent GetEvent(string id)
        {
            if (!_gameEvents.ContainsKey(id)) {
                Debug.Log($"Game Event {id} was not found in dictionary.");
                return default(GameEvent);
            }

            return _gameEvents[id];
        }

        public static GameEvent AddEvent(string id)
        {
            GameEvent newEvent = new GameEvent(id);
            _gameEvents.Add(id, newEvent);
            return newEvent;
        }
        #endregion

        #region Private Fields
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Initialize()
        {
            _gameEvents = new Dictionary<string, GameEvent>();
            LoadEventsFromResources();
        }

        private static void LoadEventsFromResources()
        {
            SOGameEvent[] events = Resources.LoadAll<SOGameEvent>("Game Events/");

            for (int i = 0; i < events.Length; i++) {
                GameEvent gameEvent = events[i].gameEvent;
                _gameEvents.Add(gameEvent.Id, gameEvent);
            }
        }
        #endregion
    }
}
