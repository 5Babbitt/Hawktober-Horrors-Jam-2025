using System;
using UnityEngine;

namespace _Scripts.TriggerVolume
{
    public class TriggerLookAt : TriggerVolume
    {
        [Header("Look At Settings")]
        [SerializeField] private Transform lookTarget;
        [SerializeField] private float lookAngleThreshold = 15f;

        private Camera cam;
        private bool playerCanLookAt;

        private void Start()
        {
            cam = Camera.main;
            
            if (!lookTarget) Debug.LogError($"No LookAt Target on {name}");
        }

        protected override void OnTriggerEnter(Collider other)
        {
            if (!CanInteract) return;
            if (!other.CompareTag("Player")) return;

            playerCanLookAt = true;
        }

        private void OnTriggerStay(Collider other)
        {
            if (!other.CompareTag("Player")) return;
            if (!playerCanLookAt) return;
            
            Vector3 directionToTarget = (lookTarget.position - cam.transform.position).normalized;
            Vector3 cameraForward = cam.transform.forward;
            
            // Calculate angle between camera forward and direction to target
            float angle = Vector3.Angle(cameraForward, directionToTarget);
            
            // Check if player is looking close enough to the target
            if (!(angle <= lookAngleThreshold)) return;
            
            RaiseTriggerEvent();
            playerCanLookAt = false; // Prevent multiple triggers until player exits and re-enters
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.CompareTag("Player")) return;
            playerCanLookAt = false;
        }
        
        protected override void OnDrawGizmos()
        {
            base.OnDrawGizmos();
            
            if (!playerCanLookAt) return;
            
            Transform cameraTransform = Camera.main.transform;
            
            // Draw line to target
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(cameraTransform.position, lookTarget.position);
            
            // Draw target marker
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(lookTarget.position, 0.5f);
            
            // Draw view cone following camera forward
            DrawViewCone(cameraTransform);
        }

        private void DrawViewCone(Transform cameraTransform)
        {
            Vector3 cameraPos = cameraTransform.position;
            Vector3 cameraForward = cameraTransform.forward; // Use camera's forward direction
            
            // Use a fixed cone distance for visualization
            float coneDistance = 10f;
            
            // Calculate cone radius at the fixed distance
            float coneRadius = coneDistance * Mathf.Tan(lookAngleThreshold * Mathf.Deg2Rad);
            
            // Draw cone outline
            int segments = 16;
            Vector3[] conePoints = new Vector3[segments];
            
            // Find perpendicular vectors to create the cone base
            Vector3 right = Vector3.Cross(cameraForward, Vector3.up).normalized;
            if (right.magnitude < 0.1f) // Handle case where direction is nearly vertical
                right = Vector3.Cross(cameraForward, Vector3.forward).normalized;
            Vector3 up = Vector3.Cross(right, cameraForward).normalized;
            
            // Calculate cone base points
            Vector3 coneCenter = cameraPos + cameraForward * coneDistance;
            for (int i = 0; i < segments; i++)
            {
                float angle = (i / (float)segments) * 2 * Mathf.PI;
                Vector3 offset = (right * Mathf.Cos(angle) + up * Mathf.Sin(angle)) * coneRadius;
                conePoints[i] = coneCenter + offset;
            }
            
            // Draw cone lines from camera to cone base
            Gizmos.color = Color.green;
            for (int i = 0; i < segments; i++)
            {
                Gizmos.DrawLine(cameraPos, conePoints[i]);
                Gizmos.DrawLine(conePoints[i], conePoints[(i + 1) % segments]);
            }
            
            // Draw angle indicator lines (showing the threshold angle)
            Gizmos.color = Color.cyan;
            Vector3 rightCone = cameraPos + (cameraForward + right * Mathf.Tan(lookAngleThreshold * Mathf.Deg2Rad)).normalized * coneDistance;
            Vector3 leftCone = cameraPos + (cameraForward - right * Mathf.Tan(lookAngleThreshold * Mathf.Deg2Rad)).normalized * coneDistance;
            Vector3 upCone = cameraPos + (cameraForward + up * Mathf.Tan(lookAngleThreshold * Mathf.Deg2Rad)).normalized * coneDistance;
            Vector3 downCone = cameraPos + (cameraForward - up * Mathf.Tan(lookAngleThreshold * Mathf.Deg2Rad)).normalized * coneDistance;
            
            Gizmos.DrawLine(cameraPos, rightCone);
            Gizmos.DrawLine(cameraPos, leftCone);
            Gizmos.DrawLine(cameraPos, upCone);
            Gizmos.DrawLine(cameraPos, downCone);
        }
    }
}