using System;
using _Scripts.InteractionSystem;
using _Scripts.SOAP.EventSystem.Events;
using UnityEngine;

namespace _Scripts.Doors
{
    [RequireComponent(typeof(HingeJoint))]
    public class SwingDoor : InteractableBehaviour
    {
        private Camera cam;
        private Rigidbody rb;
        private HingeJoint hinge;

        [Header("Door Settings")] 
        [SerializeField] private float doorSpeed;
        [SerializeField] private bool isLocked;
        [SerializeField] private bool isClosed;
        [Space(20)]
        [SerializeField] private DoorConfig config;

        public Rigidbody Rb => rb;
        public HingeJoint Hinge => hinge;

        private void Start()
        {
            cam = Camera.main;
            rb = GetComponent<Rigidbody>();
            hinge = GetComponent<HingeJoint>();

            hinge.anchor = config.anchor;
            hinge.axis = config.axis;
            hinge.massScale = config.massScale;
            hinge.useLimits = config.useLimits;
            UpdateLimits(config.min, isLocked ? config.lockedMax : config.max);
        }

        private void Update()
        {
            if (isLocked) return;
            
            if (!isClosed && InClosedPosition())
            {
                // shut door
                SetClosed(true);
            }
        }

        private void FixedUpdate()
        {
            if (!isInteracting) return;
            
            ApplyForceToDoor(doorSpeed);
        }
        
        private void ApplyForceToDoor(float speedAdd)
        {
            // Apply torque to the door around the hinge axis
            Vector3 hingeAxis = hinge.transform.TransformDirection(hinge.axis);
            rb.AddTorque(hingeAxis * (speedAdd * config.forceMultiplier * Time.fixedDeltaTime));
        }
        
        private float GetSpeedAdd()
        {
            // Get the world position of the rigidbody's center of mass
            Vector3 bodyCenter = rb.worldCenterOfMass;

            // Get the hinge joint's axis and anchor point in world space
            Vector3 hingeWorldPos = hinge.transform.TransformPoint(hinge.anchor);
            Vector3 hingeAxis = hinge.transform.TransformDirection(hinge.axis);

            // Calculate normalized vector from joint to body center
            Vector3 jointToBody = (bodyCenter - hingeWorldPos).normalized;

            // Calculate push direction based on mouse input
            Vector3 pushAmount = (cam.transform.up + cam.transform.forward) * config.mouseDelta.Value.y + cam.transform.right * config.mouseDelta.Value.x;

            // Calculate the rotation direction using cross product
            Vector3 pushRotateDir = Vector3.Cross(jointToBody, pushAmount);

            // Calculate how much the push aligns with the hinge axis
            float speedAdd = Vector3.Dot(pushRotateDir, hingeAxis);

            return speedAdd;
        }

        public void SetLocked(bool value)
        {
            isLocked = value;
            UpdateLimits(config.min, isLocked ? config.lockedMax : config.max);
            SetInteractUIText(isLocked ? config.lockedFocusText : focusText);
        }

        private void SetClosed(bool value)
        {
            isClosed = value;
        }

        private bool InClosedPosition() => Mathf.Abs(hinge.angle - hinge.limits.min) < config.closedThreshold;

        private void UpdateLimits(float min, float max)
        {
            JointLimits limits = hinge.limits;
            limits.min = min;
            limits.max = max; 
            limits.bounciness = config.bounciness;
            hinge.limits = limits;
        }

        protected override void OnFocus()
        {
            if (isLocked) SetInteractUIText(config.lockedFocusText);
        }

        protected override void OnLoseFocus() { }

        protected override void OnInteractStart()
        {
            config.toggleCameraLook.Raise(false);
        }

        protected override void OnInteractCanceled() 
        {
            config.toggleCameraLook.Raise(true);
        }

        protected override void OnInteractPerformed(float holdTime)
        {
            doorSpeed = GetSpeedAdd();
        }
    }
}