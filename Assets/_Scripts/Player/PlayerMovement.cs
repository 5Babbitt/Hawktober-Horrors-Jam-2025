using System;
using _Scripts.Utils;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Scripts.Player
{
    public class PlayerMovement : PlayerFeature
    {
        private CharacterController characterController;
        private Camera cam;

        [Header("Move Settings")]
        [SerializeField] private float walkSpeed = 3f;
        [SerializeField] private float crouchSpeed = 1.8f;
        [SerializeField] private float runSpeed = 6f;
        [SerializeField] private float playerGravity = Physics.gravity.y;
        [SerializeField] private float groundedGravity = -5f;

        [Header("Rotation Settings")] 
        [SerializeField] private float rotationDamping;

        [Header("Debug Values")]
        [SerializeField] private bool isMoving;
        [SerializeField] private bool isRunning;
        [SerializeField] private bool isCrouching;
        [SerializeField] private float moveSpeed;
        [SerializeField] private Vector2 moveInput;
        [SerializeField] private Vector3 velocity;

        [Header("Input Actions")] 
        public InputActionReference moveAction;
        public InputActionReference crouchAction;
        public InputActionReference runAction;
        public InputActionReference lookAction;

        protected override void Awake()
        {
            base.Awake();
            
            characterController = GetComponent<CharacterController>();
            cam = Camera.main;
        }

        private void OnEnable()
        {
            moveAction.action.Enable();
            crouchAction.action.Enable();
            runAction.action.Enable();
            lookAction.action.Enable();
            
            InputUtils.RegisterInputPhases(moveAction.action, OnMove);
            InputUtils.RegisterInputPhases(crouchAction.action, OnCrouch);
            InputUtils.RegisterInputPhases(runAction.action, OnRun);
            InputUtils.RegisterInputPhases(lookAction.action, OnLook);
        }

        private void OnDisable()
        {
            moveAction.action.Disable();
            crouchAction.action.Disable();
            runAction.action.Disable();
            lookAction.action.Disable();
            
            InputUtils.UnregisterInputPhases(moveAction.action, OnMove);
            InputUtils.UnregisterInputPhases(crouchAction.action, OnCrouch);
            InputUtils.UnregisterInputPhases(runAction.action, OnRun);
            InputUtils.UnregisterInputPhases(lookAction.action, OnLook);
        }

        private void Update()
        {
            HandleMovement();
            HandleGravity();
            
            characterController.Move(velocity * Time.deltaTime);
        }

        private void HandleRotation()
        {
            if (!cam) return;

            Vector3 camForward = cam.transform.forward;
            camForward.y = 0;
            
            if (camForward == Vector3.zero) return;
            
            Quaternion newRotation = Quaternion.LookRotation(camForward);
            transform.rotation = newRotation;
        }
        
        private void HandleMovement()
        {
            if (isMoving && characterController.isGrounded)
            {
                if (isCrouching) HandleCrouching();
                else HandleStanding();
            }

            Vector3 lateral = Vector3.ProjectOnPlane(transform.right, Vector3.up); // left/right
            Vector3 longitudinal = Vector3.ProjectOnPlane(transform.forward, Vector3.up); // forward/backward
            Vector3 direction = Vector3.ClampMagnitude((moveInput.x * lateral.normalized) + (moveInput.y * longitudinal.normalized), 1f);
            
            velocity.x = direction.x * moveSpeed;
            velocity.z = direction.z * moveSpeed;
        }

        private void HandleGravity()
        {
            if (characterController.isGrounded)
                velocity.y = (velocity.y < 0) ? groundedGravity * Time.deltaTime : -0.1f;
            else
                velocity.y += playerGravity * Time.deltaTime;
        }

        private void HandleStanding()
        {
            moveSpeed = isRunning ? runSpeed : walkSpeed;
        }

        private void HandleCrouching()
        {
            moveSpeed = crouchSpeed;
        }

        private void OnMove(InputAction.CallbackContext context)
        {
            moveInput = context.ReadValue<Vector2>();
            isMoving = context.performed || context.started;
        }

        private void OnCrouch(InputAction.CallbackContext context)
        {
            bool newCrouchValue = context.ReadValueAsButton();
            if (newCrouchValue == isCrouching) return;
            
            isCrouching = newCrouchValue;
            Controller.OnCrouch.Invoke(isCrouching);
        }

        private void OnRun(InputAction.CallbackContext context)
        {
            isRunning = context.ReadValueAsButton();
        }

        private void OnLook(InputAction.CallbackContext context)
        {
            HandleRotation();
        }
    }
}
