using UnityEngine;

namespace _Scripts.SOAP.EventSystem.Events
{
    [System.Serializable]
    public class FlexibleEvent
    {
        [SerializeField] private EventType eventType = EventType.SelectEventType;

        [SerializeField] private GameEvent onInteractGameEvent;
        [SerializeField] private BoolEvent onInteractBoolEvent;
        [SerializeField] private IntEvent onInteractIntEvent;
        [SerializeField] private FloatEvent onInteractFloatEvent;
        [SerializeField] private StringEvent onInteractStringEvent;

        public EventType SelectedEventType => eventType;

        public void Raise(object data = null)
        {
            switch (eventType)
            {
                case EventType.GameEvent:
                    onInteractGameEvent?.Raise();
                    break;

                case EventType.BoolEvent:
                    if (data is bool boolValue)
                        onInteractBoolEvent?.Raise(boolValue);
                    else
                        Debug.LogWarning(
                            $"InteractEvent: Expected bool data but received {data?.GetType()?.Name ?? "null"}");
                    break;

                case EventType.IntEvent:
                    if (data is int intValue)
                        onInteractIntEvent?.Raise(intValue);
                    else if (data != null && int.TryParse(data.ToString(), out int parsedInt))
                        onInteractIntEvent?.Raise(parsedInt);
                    else
                        Debug.LogWarning(
                            $"InteractEvent: Expected int data but received {data?.GetType()?.Name ?? "null"}");
                    break;

                case EventType.FloatEvent:
                    if (data is float floatValue)
                        onInteractFloatEvent?.Raise(floatValue);
                    else if (data != null && float.TryParse(data.ToString(), out float parsedFloat))
                        onInteractFloatEvent?.Raise(parsedFloat);
                    else
                        Debug.LogWarning(
                            $"InteractEvent: Expected float data but received {data?.GetType()?.Name ?? "null"}");
                    break;

                case EventType.StringEvent:
                    if (data is string stringValue)
                        onInteractStringEvent?.Raise(stringValue);
                    else
                        onInteractStringEvent?.Raise(data?.ToString() ?? "");
                    break;

                case EventType.SelectEventType:
                default:
                    // Do nothing
                    break;
            }
        }

        public enum EventType
        {
            SelectEventType = 0,
            GameEvent = 1,
            BoolEvent = 2,
            IntEvent = 3,
            FloatEvent = 4,
            StringEvent = 5
        }
    }
}