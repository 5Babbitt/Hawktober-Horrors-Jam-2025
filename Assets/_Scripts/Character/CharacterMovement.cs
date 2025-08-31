using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Scripts.Character
{
    public class CharacterMovement : MonoBehaviour
    {
        private CharacterController controller;

        [Header("Move Settings")]
        [SerializeField] private float walkSpeed = 3f;
        [SerializeField] private float crouchSpeed = 1.8f;
        [SerializeField] private float runSpeed = 6f;
        [SerializeField] private float playerGravity = Physics.gravity.y;
        [SerializeField] private float groundedGravity = -2f;

        [Header("Move Values")]
        [SerializeField] private bool isMoving;
        [SerializeField] private bool isRunning;
        [SerializeField] private bool isCrouching;
        [SerializeField] private float moveSpeed;
        [SerializeField] private Vector2 moveInput;
        [SerializeField] private Vector3 velocity;

        private void Awake()
        {
            controller = GetComponent<CharacterController>();
        }

        private void Update()
        {
            HandleMovement();
            HandleGravity();
            
            controller.Move(velocity * Time.deltaTime);
        }

        private void HandleGravity()
        {
            if (controller.isGrounded)
            {
                // Reset falling velocity and apply small downward force
                if (velocity.y < 0)
                {
                    velocity.y = groundedGravity;
                }
            }
            else
            {
                // Apply gravity acceleration
                velocity.y += playerGravity * Time.deltaTime;
            }
        }
        
        private void HandleMovement()
        {
            if (isMoving && controller.isGrounded)
            {
                if (isCrouching) HandleCrouching();
                else HandleStanding();
            }
            
            velocity.x = moveInput.x * moveSpeed;
            velocity.z = moveInput.y * moveSpeed;
        }

        private void HandleStanding()
        {
            moveSpeed = isRunning ? runSpeed : walkSpeed;
        }

        private void HandleCrouching()
        {
            moveSpeed = crouchSpeed;
        }

        private void OnMove(InputValue value)
        {
            moveInput = value.Get<Vector2>();
            isMoving = moveInput != Vector2.zero;
        }

        private void OnCrouch(InputValue value)
        {
            isCrouching = value.isPressed;
        }

        private void OnRun(InputValue value)
        {
            isRunning = value.isPressed;
        }
    }
}
