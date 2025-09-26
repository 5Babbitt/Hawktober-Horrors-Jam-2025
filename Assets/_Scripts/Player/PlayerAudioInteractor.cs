using System;
using _Scripts.Audio;
using UnityEngine;

namespace _Scripts.Player
{
    public class PlayerAudioInteractor : MonoBehaviour
    {
        [SerializeField] private LayerMask groundLayers;
        [SerializeField] private float detectionDistance = 1.1f;
        [SerializeField] private Transform detectionPoint; // Optional: specify detection point, defaults to transform position

        private SoundMaterial currentSoundMaterial;
        private SoundMaterial CurrentSoundMaterial
        {
            set => OnSoundMaterialSet(value);
            get => currentSoundMaterial;
        }

        private void Update()
        {
            DetectGroundMaterial();
        }

        private void DetectGroundMaterial()
        {
            Vector3 rayOrigin = detectionPoint ? detectionPoint.position : transform.position;
        
            if (Physics.Raycast(rayOrigin, Vector3.down, out RaycastHit hit, detectionDistance, groundLayers))
            {
                // Try to get SoundMaterial from the hit object
                SoundMaterial soundMat = hit.collider.GetComponent<SoundMaterial>();
            
                if (soundMat)
                {
                    CurrentSoundMaterial = soundMat;
                }
                else
                {
                    // No SoundMaterial found, set to null (default/silent)
                    CurrentSoundMaterial = null;
                }
            }
            else
            {
                // No ground detected, set to null
                CurrentSoundMaterial = null;
            }
        }

        private void OnSoundMaterialSet(SoundMaterial newMat)
        {
            if (newMat == currentSoundMaterial) return;
        
            currentSoundMaterial = newMat;
        
            newMat?.Material?.SetValue(gameObject);
        }
        

        private void OnDrawGizmos()
        {
            Vector3 rayOrigin = !detectionPoint ? detectionPoint.position : transform.position;
        
            Gizmos.color = !currentSoundMaterial ? Color.green : Color.red;
            Gizmos.DrawRay(rayOrigin, Vector3.down * detectionDistance);
        }
    }
    
}