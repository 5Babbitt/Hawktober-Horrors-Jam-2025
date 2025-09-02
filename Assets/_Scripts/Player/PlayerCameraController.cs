using System;
using Unity.Cinemachine;
using UnityEngine;

namespace _Scripts.Player
{
    public class PlayerCameraController : PlayerFeature
    {
        private CinemachineCamera cineCam;
        private CinemachineFollow camFollow;

        [Header("Player Camera Settings")]
        [SerializeField] private float standHeight = 0.75f;
        [SerializeField] private float crouchHeight = 0.5f;

        protected override void Awake()
        {
            base.Awake();

            cineCam = GetComponentInChildren<CinemachineCamera>();
            camFollow = cineCam.gameObject.GetComponent<CinemachineFollow>();
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
            camFollow.FollowOffset.y = isCrouching ? crouchHeight : standHeight;
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
