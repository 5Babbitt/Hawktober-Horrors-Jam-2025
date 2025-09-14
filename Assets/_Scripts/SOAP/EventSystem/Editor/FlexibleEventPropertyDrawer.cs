using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using _Scripts.SOAP.EventSystem.Events;

namespace _Scripts.SOAP.EventSystem.Editor
{
    [CustomPropertyDrawer(typeof(FlexibleEvent))]
    public class FlexibleEventPropertyDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            // Create container
            var container = new VisualElement();

            // Get properties
            var eventTypeProp = property.FindPropertyRelative("eventType");
            var gameEventProp = property.FindPropertyRelative("onInteractGameEvent");
            var boolEventProp = property.FindPropertyRelative("onInteractBoolEvent");
            var intEventProp = property.FindPropertyRelative("onInteractIntEvent");
            var floatEventProp = property.FindPropertyRelative("onInteractFloatEvent");
            var stringEventProp = property.FindPropertyRelative("onInteractStringEvent");

            // Create event type dropdown
            var eventTypeField = new PropertyField(eventTypeProp, property.displayName);
            container.Add(eventTypeField);

            // Create container for event fields
            var eventFieldContainer = new VisualElement();
            container.Add(eventFieldContainer);

            // Method to update the displayed event field
            void UpdateEventField()
            {
                eventFieldContainer.Clear();

                var eventType = (FlexibleEvent.EventType)eventTypeProp.enumValueIndex;

                switch (eventType)
                {
                    case FlexibleEvent.EventType.GameEvent:
                        var gameEventField = new PropertyField(gameEventProp, "Game Event");
                        gameEventField.BindProperty(gameEventProp);
                        eventFieldContainer.Add(gameEventField);
                        break;

                    case FlexibleEvent.EventType.BoolEvent:
                        var boolEventField = new PropertyField(boolEventProp, "Bool Event");
                        boolEventField.BindProperty(boolEventProp);
                        eventFieldContainer.Add(boolEventField);
                        break;

                    case FlexibleEvent.EventType.IntEvent:
                        var intEventField = new PropertyField(intEventProp, "Int Event");
                        intEventField.BindProperty(intEventProp);
                        eventFieldContainer.Add(intEventField);
                        break;

                    case FlexibleEvent.EventType.FloatEvent:
                        var floatEventField = new PropertyField(floatEventProp, "Float Event");
                        floatEventField.BindProperty(floatEventProp);
                        eventFieldContainer.Add(floatEventField);
                        break;

                    case FlexibleEvent.EventType.StringEvent:
                        var stringEventField = new PropertyField(stringEventProp, "String Event");
                        stringEventField.BindProperty(stringEventProp);
                        eventFieldContainer.Add(stringEventField);
                        break;

                    case FlexibleEvent.EventType.SelectEventType:
                    default:
                        var helpLabel = new Label("Select an event type above");
                        helpLabel.style.unityFontStyleAndWeight = FontStyle.Italic;
                        helpLabel.style.color = new Color(0.7f, 0.7f, 0.7f, 1f);
                        eventFieldContainer.Add(helpLabel);
                        break;
                }
            }

            // Initial update
            UpdateEventField();

            // Register callback for when event type changes
            eventTypeField.RegisterValueChangeCallback(evt => UpdateEventField());

            return container;
        }
    }
}