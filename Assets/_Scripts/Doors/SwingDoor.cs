using System;
using _Scripts.InteractionSystem;
using _Scripts.InventorySystem;
using _Scripts.SOAP.EventSystem.Events;
using _Scripts.SOAP.Variables;
using UnityEngine;
using UnityEngine.Events;

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
        
        [Header("Lock Settings")]
        [SerializeField] private bool isLocked;
        [SerializeField] private StringVariable requiredKeyId;
        
        [Space(20)]
        [SerializeField] private DoorConfig config;
        
        private bool wasClosed;

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
            
            wasClosed = InClosedPosition();
        }

        private void Update()
        {
            if (isLocked) return;
            
            HandleCloseDetection();
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

        private void SetLocked(bool value)
        {
            isLocked = value;
            UpdateLimits(config.min, isLocked ? config.lockedMax : config.max);
            SetInteractUIText(isLocked ? config.lockedFocusText : focusText);
        }


        private void UpdateLimits(float min, float max)
        {
            JointLimits limits = hinge.limits;
            limits.min = min;
            limits.max = max; 
            limits.bounciness = config.bounciness;
            hinge.limits = limits;
        }
        
        #region Open/Close Methods
        private bool InClosedPosition() => Mathf.Abs(hinge.angle - hinge.limits.min) < config.closedThreshold;
        
        private void HandleCloseDetection()
        {
            bool currentlyInClosedPosition = InClosedPosition();
            
            // Check if door state changed
            if (currentlyInClosedPosition != wasClosed)
            {
                if (currentlyInClosedPosition)
                {
                    // Door just reached closed position
                    HandleDoorClosed();
                }
                else
                {
                    // Door just left closed position
                    HandleDoorOpened();
                }
                
                wasClosed = currentlyInClosedPosition;
            }
        }

        private void HandleDoorClosed()
        {
            SimpleSnapToClosed();
            Debug.Log($"Door {name} closed");
        }

        private void HandleDoorOpened()
        {
            Debug.Log($"Door {name} opened");
        }
        
        void SimpleSnapToClosed()
        {
            float currentAngle = hinge.angle;
            float targetAngle = hinge.limits.min;
            float angleDifference = targetAngle - currentAngle;

            if (!(Mathf.Abs(angleDifference) > 0.5f)) return;
            
            Vector3 hingeAxis = hinge.transform.TransformDirection(hinge.axis);
            rb.AddTorque(hingeAxis * (angleDifference * 2f), ForceMode.VelocityChange);
        }
        #endregion

        #region Lock Methods
        private bool TryUnlock()
        {
            if (!isLocked) return true; // Already unlocked

            if (!requiredKeyId)
            {
                Debug.LogError($"locked door {name} has no required key");
                return false;
            }
            
            // Check if provided key matches
            if (!Inventory.Instance.HasItem(requiredKeyId.Value))
            {
                // No key provided
                SetInteractUIText(config.noKeyText);
                return false;
            }
    
            // Unlock successful
            SetLocked(false);
            SetInteractUIText(config.unlockSuccessText);
    
            // TODO Raise unlock event for audio/effects
    
            Debug.Log($"Door {name} unlocked with key: {requiredKeyId?.Value}");
            return true;
        }

        #endregion

        #region Interaction Methods
        protected override void OnFocus()
        {
            if (isLocked) SetInteractUIText(config.lockedFocusText);
        }

        protected override void OnLoseFocus() { }

        protected override void OnInteractStart()
        {
            if (!TryUnlock())
            {
                // Unlock failed - don't disable camera look
            }
            else
            {
                // Unlock Successful
            }
            
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
        #endregion
    }
}