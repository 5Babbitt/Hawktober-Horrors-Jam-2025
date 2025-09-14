using System.Collections;
using _Scripts.SOAP.EventSystem.Events;
using _Scripts.Utils;
using UnityEngine;
using UnityEngine.InputSystem;
using _Scripts.Doors;

namespace _Scripts.Player
{
    public class PlayerDoorInteractor : MonoBehaviour
    {
        private const float ReachCheckTime = 0.2f;
        
        private Camera cam;
        [SerializeField] private float reach;
        [SerializeField] private float forceMultiplier;
        [SerializeField] private LayerMask doorLayer;
        [SerializeField] private BoolEvent enableCameraLook;

        [Space(20)] 
        [SerializeField] private Vector2 mouseDelta;
        [SerializeField] private SwingDoor selectedDoor;
        
        private float doorSpeed;
        private Coroutine doorInReachCoroutine;
    
        [Header("Input Actions")] 
        public InputActionReference interactAction;
        public InputActionReference lookAction;

        private void Awake()
        {
            cam = Camera.main;
            
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void OnEnable()
        {
            interactAction.action.Enable();
            lookAction.action.Enable();
            InputUtils.RegisterInputPhases(interactAction.action, OnInteract);
            InputUtils.RegisterInputPhases(lookAction.action, OnLook);
        }

        private void OnDisable()
        {
            interactAction.action.Disable();
            lookAction.action.Disable();
            InputUtils.UnregisterInputPhases(interactAction.action, OnInteract);
            InputUtils.UnregisterInputPhases(lookAction.action, OnLook);
        }

        private void Update()
        {
            if (!selectedDoor) return;
            if (Vector3.Distance(selectedDoor.transform.position, transform.position) > reach)
            {
                ClearSelectedDoor();
                return;
            }
            
            doorSpeed = GetSpeedAdd();
        }

        private void FixedUpdate()
        {
            if (!selectedDoor) return;
            ApplyForceToDoor(doorSpeed);
        }
        
        private float GetSpeedAdd()
        {
            // Get the world position of the rigidbody's center of mass
            Vector3 bodyCenter = selectedDoor.Rb.worldCenterOfMass;

            // Get the hinge joint's axis and anchor point in world space
            Vector3 hingeWorldPos = selectedDoor.Hinge.transform.TransformPoint(selectedDoor.Hinge.anchor);
            Vector3 hingeAxis = selectedDoor.Hinge.transform.TransformDirection(selectedDoor.Hinge.axis);

            // Calculate normalized vector from joint to body center
            Vector3 jointToBody = (bodyCenter - hingeWorldPos).normalized;

            // Calculate push direction based on mouse input
            Vector3 pushAmount = (cam.transform.up + cam.transform.forward) * mouseDelta.y + cam.transform.right * mouseDelta.x;

            // Calculate the rotation direction using cross product
            Vector3 pushRotateDir = Vector3.Cross(jointToBody, pushAmount);

            // Calculate how much the push aligns with the hinge axis
            float speedAdd = Vector3.Dot(pushRotateDir, hingeAxis);

            return speedAdd;
        }

        private void ApplyForceToDoor(float speedAdd)
        {
            // Apply torque to the door around the hinge axis
            Vector3 hingeAxis = selectedDoor.Hinge.transform.TransformDirection(selectedDoor.Hinge.axis);
            selectedDoor.Rb.AddTorque(hingeAxis * (speedAdd * forceMultiplier * Time.fixedDeltaTime));
        }

        private bool CastForDoor(out RaycastHit hit)
        {
            return Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, reach, doorLayer);
        }

        private void OnInteract(InputAction.CallbackContext context)
        {
            bool isPressed = context.ReadValueAsButton();

            switch (isPressed)
            {
                case true when !selectedDoor && CastForDoor(out RaycastHit hit):
                    GetSelectedDoor(hit);
                    break;
                case false when selectedDoor != null:
                    ClearSelectedDoor();
                    break;
            }
        }

        private void GetSelectedDoor(RaycastHit hit)
        {
            selectedDoor = hit.transform.GetComponent<SwingDoor>();
            enableCameraLook.Raise(false);
            doorInReachCoroutine = StartCoroutine(CheckDoorInReach());
        }

        private void ClearSelectedDoor()
        {
            selectedDoor = null;
            enableCameraLook.Raise(true);
            if (doorInReachCoroutine != null)
            {
                StopCoroutine(doorInReachCoroutine);
            }
        }
        
        private void OnLook(InputAction.CallbackContext context)
        {
            mouseDelta = context.ReadValue<Vector2>();
        }

        private IEnumerator CheckDoorInReach()
        {
            while (true)
            {
                if (Vector3.Distance(selectedDoor.transform.position, transform.position) > reach)
                {
                    ClearSelectedDoor();
                }

                yield return new WaitForSeconds(ReachCheckTime);
            }
            
        }
    }
}

