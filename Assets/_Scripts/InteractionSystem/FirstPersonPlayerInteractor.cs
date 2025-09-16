using System;
using _Scripts.Utils;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Scripts.InteractionSystem
{
    public class FirstPersonPlayerInteractor : MonoBehaviour
    {
        private Camera cam;
        private IInteractable currentInteractable;
        private float interactionStartTime;
        private bool isInteracting;

        [SerializeField] private bool canInteract = true;
        [SerializeField] private float maxInteractionDistance = 5f;
        
        [Header("Raycast Settings")]
        [SerializeField] private float maxInteractDistance;
        [SerializeField] private LayerMask interactableLayers;
        
        [Header("Input Settings")] 
        [SerializeField] private bool interactPressed;
        public InputActionReference interactAction;

        protected void Awake()
        {
            cam = Camera.main;
            
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void OnEnable()
        {
            interactAction.action.Enable();
            InputUtils.RegisterInputPhases(interactAction.action, OnInteract, InputPhases.Started | InputPhases.Canceled);
        }

        private void OnDisable()
        {
            interactAction.action.Disable();
            InputUtils.UnregisterInputPhases(interactAction.action, OnInteract);
        }

        private void Update()
        {
            DetectInteractable();
            PerformInteraction();
        }

        void DetectInteractable()
        {
            if (Physics.Raycast(cam.transform.position, cam.transform.forward, out RaycastHit hit, maxInteractDistance, interactableLayers))
            {
                if (!hit.collider.TryGetComponent(out IInteractable newInteractable)) return;
                if (currentInteractable == newInteractable || !newInteractable.CanInteract) return;
                
                currentInteractable = newInteractable;
                currentInteractable?.Focus();
            }
            else if (currentInteractable != null && !isInteracting)
            {
                currentInteractable.LoseFocus();
                currentInteractable = null;
            }
            
            if (currentInteractable == null) return;

            if (!(Vector3.Distance(transform.position, currentInteractable.Transform.position) > maxInteractionDistance)) return;
            if (isInteracting) CancelInteraction();
            currentInteractable.LoseFocus();
            currentInteractable = null;
        }

        private void PerformInteraction()
        {
            if (!isInteracting || currentInteractable == null) return;
            float holdTime = Time.time - interactionStartTime;
            currentInteractable.InteractPerform(holdTime);
        }

        private void StartInteraction()
        {
            if (!canInteract || !currentInteractable.CanInteract) return;

            interactionStartTime = Time.time;
            isInteracting = true;
            
            currentInteractable?.InteractStart();
        }

        private void CancelInteraction()
        {
            if (!isInteracting) return;
            
            isInteracting = false;
            
            currentInteractable?.InteractCancel();
        }

        private void OnInteract(InputAction.CallbackContext context)
        {
            interactPressed = context.ReadValueAsButton();
            
            if (currentInteractable == null || !canInteract) return;
            if (interactPressed) 
                StartInteraction();
            else
                CancelInteraction();
        }

        private void OnDrawGizmosSelected()
        {
            Transform camTransform = Camera.main.transform;
            
            Gizmos.color = Color.blueViolet;
            Gizmos.DrawLine(camTransform.position, camTransform.position + camTransform.forward * maxInteractDistance);
            Gizmos.DrawSphere(camTransform.position + (camTransform.forward * maxInteractDistance), 0.1f);
        }
    }
}