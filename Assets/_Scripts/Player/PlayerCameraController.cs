using System;
using System.Collections;
using _Scripts.SOAP.EventSystem.Events;
using Unity.Cinemachine;
using UnityEngine;

namespace _Scripts.Player
{
    public class PlayerCameraController : PlayerFeature
    {
        private const string LookX = "Look X (Pan)";
        private const string LookY = "Look Y (Tilt)";
        
        private CinemachineCamera cineCam;
        private CinemachineFollow camFollow;
        private CinemachineInputAxisController camInput;

        [Header("Crouch Camera Settings")]
        [SerializeField] private float standHeight = 0.75f;
        [SerializeField] private float crouchHeight = 0.5f;
        [SerializeField] private float crouchTime = 0.5f;
        [SerializeField] private AnimationCurve crouchTransitionCurve;

        private Coroutine crouchCoroutine;

        protected override void Awake()
        {
            base.Awake();

            cineCam = GetComponentInChildren<CinemachineCamera>();
            camFollow = cineCam.gameObject.GetComponent<CinemachineFollow>();
            camInput = cineCam.gameObject.GetComponent<CinemachineInputAxisController>();
        }

        private void OnEnable()
        {
            Controller.OnCrouch += OnCrouch;
        }
        
        private void OnDisable()
        {
            Controller.OnCrouch -= OnCrouch;
        }

        private void Start()
        {
            camFollow.FollowOffset.y = standHeight;
        }

        private void OnCrouch(bool isCrouching)
        {
            if (crouchCoroutine != null)
            {
                StopCoroutine(crouchCoroutine);
            }
            
            float targetHeight = isCrouching ? crouchHeight : standHeight;
            crouchCoroutine = StartCoroutine(CrouchTransition(targetHeight));
        }

        public void SetCameraFreelook(bool canLook)
        {
            SetCameraInputEnabled(canLook);
        }

        private void SetCameraInputEnabled(bool isEnabled, string axisName = "")
        {
            foreach (var inputAxis in camInput.Controllers)
            {
                if (string.IsNullOrEmpty(axisName) || inputAxis.Name == axisName)
                    inputAxis.Enabled = isEnabled;
            }
        }

        private IEnumerator CrouchTransition(float targetHeight)
        {
            float startHeight = camFollow.FollowOffset.y;
            float elapsedTime = 0f;

            while (elapsedTime < crouchTime)
            {
                elapsedTime += Time.deltaTime;
                float progress = elapsedTime / crouchTime;

                progress = crouchTransitionCurve.Evaluate(progress);

                camFollow.FollowOffset.y = Mathf.Lerp(startHeight, targetHeight, progress);

                yield return null;
            }

            camFollow.FollowOffset.y = targetHeight;
            crouchCoroutine = null;
        }

        private void OnValidate()
        {
            if (!cineCam) cineCam = GetComponentInChildren<CinemachineCamera>();
            if (!camFollow) camFollow = cineCam.gameObject.GetComponent<CinemachineFollow>();

            if (!Mathf.Approximately(camFollow.FollowOffset.y, standHeight)) 
                camFollow.FollowOffset.y = standHeight;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(transform.root.position + (Vector3.up * crouchHeight), 0.25f);
        }
    }
}
