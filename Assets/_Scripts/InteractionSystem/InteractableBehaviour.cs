using _Scripts.SOAP.EventSystem;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Scripts.InteractionSystem
{
    public class InteractableBehaviour : MonoBehaviour, IInteractable
    {
        public GameEvent onInteractEvent;
        
        public void OnFocus()
        {
            Debug.Log($"Looking at {name}");
        }

        public void OnLostFocus()
        {
            Debug.Log($"Lost focus of {name}");
        }

        public void OnInteract()
        {
            Debug.Log($"Interacted with {name}");
            onInteractEvent.Raise();
        }
    }
}