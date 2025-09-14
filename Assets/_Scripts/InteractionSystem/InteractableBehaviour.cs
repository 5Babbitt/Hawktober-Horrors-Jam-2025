using _Scripts.SOAP.Variables;
using UnityEngine;
using UnityEngine.Events;

namespace _Scripts.InteractionSystem
{
    public class InteractableBehaviour : MonoBehaviour, IInteractable
    {
        [SerializeField] protected string focusMessage = "Interact With Object";
        [SerializeField] protected bool isSingleUse = false;

        public StringVariable interactUIMessage;

        [Header("Events")]
        public UnityEvent onInteractEvent;

        protected bool used = false;

        protected bool CanInteract => isSingleUse && used;
        
        public virtual void OnFocus()
        {
            interactUIMessage.Value = focusMessage;
        }

        public virtual void OnLostFocus()
        {
            interactUIMessage.Value = "";
        }

        public virtual bool OnInteract()
        {
            interactUIMessage.Value = "";
            if (CanInteract) return false;
            onInteractEvent?.Invoke();
            if (isSingleUse) used = true;
            return true;
        }
    }
}