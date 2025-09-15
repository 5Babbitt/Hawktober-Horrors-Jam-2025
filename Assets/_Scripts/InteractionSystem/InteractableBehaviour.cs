using System;
using _Scripts.SOAP.Variables;
using UnityEngine;
using UnityEngine.Events;

namespace _Scripts.InteractionSystem
{
    public abstract class InteractableBehaviour : MonoBehaviour, IInteractable
    {
        [Header("Interaction Settings")] 
        [SerializeField] protected EInteractionType interactionType = EInteractionType.Click;
        [SerializeField] protected bool canInteract = true;
        [SerializeField] protected bool isSingleUse = false;
        
        [Header("Interact UI Settings")]
        [SerializeField] protected string focusText = "Interact with object";
        [SerializeField] protected StringVariable interactUIText;

        protected bool isInteracting = false;
        protected bool isFocused = false;

        public bool CanInteract => canInteract;
        public Transform Transform => transform;

        public void Focus()
        {
            SetInteractUIText(focusText);
            isFocused = true;
            OnFocus();
        }

        public void LoseFocus()
        {
            ClearInteractUIText();
            isFocused = false;
            OnLoseFocus();
        }

        public void InteractStart()
        {
            if (!CanInteract) return;
            
            ClearInteractUIText();
            isInteracting = true;
            OnInteractStart();
        }

        public void InteractCancel()
        {
            if (!isInteracting) return;
            Focus();
            isInteracting = false;
            OnInteractCanceled();
            if (isSingleUse) canInteract = false;
        }

        public void InteractPerform(float holdTime)
        {
            if (interactionType != EInteractionType.ClickAndHold && (!CanInteract || !isInteracting)) return;

            OnInteractPerformed(holdTime);
        }

        protected void SetInteractUIText(string text) => interactUIText.Value = text;
        protected void ClearInteractUIText() => interactUIText.Value = string.Empty;

        // Abstract methods for derived classes to implement
        protected abstract void OnFocus();
        protected abstract void OnLoseFocus();
        protected abstract void OnInteractStart();
        protected abstract void OnInteractCanceled();
        protected abstract void OnInteractPerformed(float holdTime);
    }
}