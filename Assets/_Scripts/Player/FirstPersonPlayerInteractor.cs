using System;
using _Scripts.InteractionSystem;
using _Scripts.Utils;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Scripts.Player
{
    public class FirstPersonPlayerInteractor : PlayerFeature
    {
        private Camera cam;

        [SerializeField] private bool canInteract = true;
        [SerializeField] private bool interactPressed;
        [SerializeField] private float maxInteractDistance;
        [SerializeField] private LayerMask interactableLayers;
        [SerializeField] private string focusMessage;
        
        private IInteractable currentInteractable;
            
        [Header("Input Actions")] 
        public InputActionReference interactAction;

        protected override void Awake()
        {
            base.Awake();
            
            cam = Camera.main;
        }

        private void OnEnable()
        {
            interactAction.action.Enable();
            InputUtils.RegisterInputPhases(interactAction.action, OnInteract);
        }

        private void OnDisable()
        {
            interactAction.action.Disable();
            InputUtils.UnregisterInputPhases(interactAction.action, OnInteract);
        }

        private void FixedUpdate()
        {
            if (Physics.Raycast(cam.transform.position, cam.transform.forward, out RaycastHit hit, maxInteractDistance, interactableLayers))
            {
                if (!hit.collider.TryGetComponent(out IInteractable newInteractable)) return;
                if (currentInteractable == newInteractable) return;
                
                currentInteractable = newInteractable;
                currentInteractable?.OnFocus();
            }
            else if (currentInteractable != null)
            {
                currentInteractable.OnLostFocus();
                currentInteractable = null;
            }
        }

        private void OnInteract(InputAction.CallbackContext context)
        {
            interactPressed = context.ReadValueAsButton();
            if (canInteract && interactPressed) currentInteractable?.OnInteract();
        }

        private void OnDrawGizmosSelected()
        {
            Transform cameraTransform = Camera.main.transform;
            
            Gizmos.color = Color.blueViolet;
            Gizmos.DrawLine(cameraTransform.position, cameraTransform.position + cameraTransform.forward * maxInteractDistance);
            Gizmos.DrawSphere(cameraTransform.position + (cameraTransform.forward * maxInteractDistance), 0.1f);
        }
    }
}