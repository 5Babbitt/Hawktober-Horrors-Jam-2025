using System;
using _Scripts.SOAP.Variables;
using _Scripts.Utils;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Scripts.Player
{
    public class PlayerMovement : MonoBehaviour
    {
        private CharacterController characterController;
        private Camera cam;

        [Header("Move Settings")]
        [SerializeField] private float walkSpeed = 3f;
        [SerializeField] private float crouchSpeed = 1.8f;
        [SerializeField] private float runSpeed = 6f;
        [SerializeField] private float playerGravity = Physics.gravity.y;
        [SerializeField] private float groundedGravity = -5f;
        [SerializeField] private Vector3Variable playerPos;
        [SerializeField] private Vector2Variable lookDelta;
        [SerializeField] private BoolVariable playerCrouched;

        [Header("Test Movement Settings")]
        [SerializeField] private bool enableTestMovement;
        [SerializeField] private Transform[] testWaypoints;
        [SerializeField] private float waypointReachedDistance = 0.5f;
        private int currentWaypointIndex = 0;
        private float testStartTime;
        private bool isTestMovementActive;

        [Header("Sound Settings")] 
        [SerializeField] private float timeBetweenFootsteps;
        [SerializeField] private AK.Wwise.Event footstep;
        private float timeSinceLastStep;

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

        private void Awake()
        {
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
            if (enableTestMovement)
            {
                HandleTestMovement();
            }
            
            HandleMovement();
            HandleGravity();
            
            characterController.Move(velocity * Time.deltaTime);
            playerPos.Value = transform.position;
        }

        private void HandleRotation()
        {
            if (!cam) return;
            if (enableTestMovement) return; 

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
                if (isCrouching) HandleCrouchedMovement();
                else HandleStandingMovement();

                FootstepsUpdate(isCrouching);
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

        private void HandleStandingMovement()
        {
            moveSpeed = isRunning ? runSpeed : walkSpeed;
        }

        private void HandleCrouchedMovement()
        {
            moveSpeed = crouchSpeed;
        }

        private void FootstepsUpdate(bool crouching)
        {
            timeSinceLastStep += Time.deltaTime;
            if (timeSinceLastStep > timeBetweenFootsteps)
            {
                footstep.Post(gameObject);
                timeSinceLastStep = 0;
            }
        }

        private void OnMove(InputAction.CallbackContext context)
        {
            if (enableTestMovement) return; // Ignore manual input during test
            
            moveInput = context.ReadValue<Vector2>();
            isMoving = context.performed || context.started;
        }

        private void OnCrouch(InputAction.CallbackContext context)
        {
            bool newCrouchValue = context.ReadValueAsButton();
            if (newCrouchValue == isCrouching) return;
            
            isCrouching = newCrouchValue;
            playerCrouched.Value = isCrouching;
            
            isRunning = false;
        }

        private void OnRun(InputAction.CallbackContext context)
        {
            if (isCrouching) return;
            
            isRunning = context.ReadValueAsButton();
        }

        private void OnLook(InputAction.CallbackContext context)
        {
            HandleRotation();
            lookDelta.Value = context.ReadValue<Vector2>();
        }
        
        

        [ContextMenu("Start Test Movement")]
        public void StartTestMovement()
        {
            if (testWaypoints == null || testWaypoints.Length == 0)
            {
                Debug.LogWarning("No test waypoints assigned!");
                return;
            }
            
            enableTestMovement = true;
            currentWaypointIndex = 0;
            isTestMovementActive = false;
        }

        [ContextMenu("Stop Test Movement")]
        public void StopTestMovement()
        {
            enableTestMovement = false;
            isTestMovementActive = false;
            isMoving = false;
            moveInput = Vector2.zero;
            currentWaypointIndex = 0;
            Debug.Log("Test movement stopped");
        }
        
        private void HandleTestMovement()
        {
            if (testWaypoints == null || testWaypoints.Length == 0) return;
            
            if (!isTestMovementActive)
            {
                isTestMovementActive = true;
                testStartTime = Time.time;
                Debug.Log($"Test movement started at {TimerUtils.FloatToTime(testStartTime)}");
            }

            Transform targetWaypoint = testWaypoints[currentWaypointIndex];
            Vector3 targetPos = targetWaypoint.position;
            targetPos.y = transform.position.y; // Keep on same height plane
            
            Vector3 directionToWaypoint = (targetPos - transform.position).normalized;
            
            // Rotate to face the waypoint
            if (directionToWaypoint != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(directionToWaypoint);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
            }
            
            // Set movement input to move toward waypoint
            Vector3 localDirection = transform.InverseTransformDirection(directionToWaypoint);
            moveInput = new Vector2(localDirection.x, localDirection.z);
            isMoving = true;
            
            // Check if reached waypoint
            float distanceToWaypoint = Vector3.Distance(transform.position, targetPos);
            if (distanceToWaypoint <= waypointReachedDistance)
            {
                currentWaypointIndex++;
                
                if (currentWaypointIndex >= testWaypoints.Length)
                {
                    // Completed all waypoints
                    float totalTime = Time.time - testStartTime;
                    Debug.Log($"Test movement completed! Total time: {TimerUtils.FloatToTime(totalTime)} ({totalTime:F2} seconds)");
                    enableTestMovement = false;
                    isTestMovementActive = false;
                    isMoving = false;
                    moveInput = Vector2.zero;
                    currentWaypointIndex = 0;
                }
                else
                {
                    float elapsedTime = Time.time - testStartTime;
                    Debug.Log($"Reached waypoint {currentWaypointIndex} at {TimerUtils.FloatToTime(elapsedTime)} ({elapsedTime:F2} seconds)");
                }
            }
        }
    }
}
