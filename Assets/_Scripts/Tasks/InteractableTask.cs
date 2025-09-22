using _Scripts.InteractionSystem;
using _Scripts.SOAP.EventSystem.Events;
using UnityEngine;

namespace _Scripts.Tasks
{
    public class InteractableTask : InteractableBehaviour
    {
        [SerializeField] private BoolEvent togglePlayerSystems;
        
        protected override void OnFocus()
        {
            
        }

        protected override void OnLoseFocus()
        {
            
        }

        protected override void OnInteractStart()
        {
            togglePlayerSystems.Raise(!isToggled);
        }

        protected override void OnInteractCanceled()
        {
            
        }

        protected override void OnInteractPerformed(float holdTime) { }
    }
}