using System;
using _Scripts.SOAP.EventSystem.Events;
using Unity.VisualScripting;
using UnityEngine;

namespace _Scripts.TriggerVolume
{
    [RequireComponent(typeof(BoxCollider), typeof(Rigidbody))]
    public class TriggerVolume : MonoBehaviour
    {
        [SerializeField] private int numTimesTriggered;
        [SerializeField] private int triggerLimit = 1;
        [SerializeField] private FlexibleEvent triggerEvent;

        private new BoxCollider collider;

        private bool CanInteract => triggerLimit > 0 && numTimesTriggered < triggerLimit || triggerLimit == 0;

        private void Awake()
        { 
            Rigidbody rb = GetComponent<Rigidbody>();
            collider = GetComponent<BoxCollider>();

            rb.isKinematic = true;
            
            gameObject.tag = "Trigger";
            numTimesTriggered = 0;
            collider.isTrigger = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!CanInteract) return;
            if (!other.CompareTag("Player")) return;
            
            RaiseTriggerEvent();
        }

        private void RaiseTriggerEvent()
        {
            Debug.Log("Triggered");
            
            numTimesTriggered++;
            triggerEvent.Raise();
        }

        private void OnDrawGizmos()
        {
            if (!collider) collider = GetComponent<BoxCollider>();
            
            Gizmos.color = Color.mediumSpringGreen;
            Gizmos.DrawWireCube(transform.position, collider.size);
        }
    }
}
