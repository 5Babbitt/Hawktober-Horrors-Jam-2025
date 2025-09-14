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
        [SerializeField] private bool isLocked;
        [SerializeField] private bool isClosed;
        [Space(20)]
        [SerializeField] private DoorConfig config;

        public Rigidbody Rb => rb;
        public HingeJoint Hinge => hinge;

        private void Start()
        {
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

        public override void OnFocus()
        {
            interactUIMessage.Value = isLocked ? config.lockedFocusMessage : focusMessage;
        }

        public override bool OnInteract()
        {
            return base.OnInteract();
        }

        public void SetLocked(bool value)
        {
            isLocked = value;
            UpdateLimits(config.min, isLocked ? config.lockedMax : config.max);
        }

        public void SetClosed(bool value)
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
    }
}