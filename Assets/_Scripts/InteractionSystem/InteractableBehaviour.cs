using _Scripts.SOAP.Variables;
using UnityEngine;
using UnityEngine.Events;

namespace _Scripts.InteractionSystem
{
    public class InteractableBehaviour : MonoBehaviour, IInteractable
    {
        [SerializeField] private string focusMessage;

        public StringVariable interactUIMessage;
        public UnityEvent<object> onInteractEvent;
        
        public void OnFocus()
        {
            Debug.Log($"Looking at {name}");
            interactUIMessage.Value = focusMessage;
        }

        public void OnLostFocus()
        {
            Debug.Log($"Lost focus of {name}");
            interactUIMessage.Value = "";
        }

        public void OnInteract(object data = null)
        {
            Debug.Log($"Interacted with {name}");
            onInteractEvent.Invoke(data);
        }
    }
}