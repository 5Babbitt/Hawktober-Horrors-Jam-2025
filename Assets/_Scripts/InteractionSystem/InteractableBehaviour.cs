using _Scripts.SOAP.Variables;
using _Scripts.SOAP.EventSystem.Events;
using UnityEngine;

namespace _Scripts.InteractionSystem
{
    public class InteractableBehaviour : MonoBehaviour, IInteractable
    {
        [SerializeField] private string focusMessage = "Interact With Object";
        [SerializeField] private bool isSingleUse = false;

        public StringVariable interactUIMessage;

        [Header("Events")] public FlexibleEvent onInteractEvent;

        private bool used = false;

        public void OnFocus()
        {
            interactUIMessage.Value = focusMessage;
        }

        public void OnLostFocus()
        {
            interactUIMessage.Value = "";
        }

        public void OnInteract(object data = null)
        {
            if (isSingleUse && used) return;
            onInteractEvent?.Raise(data);
            if (isSingleUse) used = true;
        }
    }
}