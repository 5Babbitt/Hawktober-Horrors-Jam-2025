using System.Collections.Generic;
using UnityEngine;

namespace _Scripts.SOAP.EventSystem
{
    public class GameEvent<T> : ScriptableObject
    {
        private readonly List<IGameEventListener<T>> listeners = new();

        public void Raise(T data)
        {
            for (var i = listeners.Count - 1; i >= 0; i--)
            {
                listeners[i].OnEventRaised(data);
            }
        }

        public void RegisterListener(IGameEventListener<T> listener) => listeners.Add(listener);
        public void DeregisterListener(IGameEventListener<T> listener) => listeners.Remove(listener);
    }

    [CreateAssetMenu(fileName = "GameEvent", menuName = "Events/Game Event")]
    public class GameEvent : GameEvent<Unit>
    {
        public void Raise() => Raise(Unit.Default);
    }

    /// <summary>
    /// A null object used in empty game events
    /// </summary>
    public struct Unit
    {
        public static Unit Default => default;
    }
}
