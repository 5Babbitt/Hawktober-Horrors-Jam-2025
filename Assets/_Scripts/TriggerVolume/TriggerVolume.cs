using System;
using _Scripts.SOAP.EventSystem.Events;
using UnityEngine;

namespace _Scripts.TriggerVolume
{
    [RequireComponent(typeof(BoxCollider), typeof(Rigidbody))]
    public class TriggerVolume : MonoBehaviour
    {
        [Header("Trigger Settings")]
        [SerializeField] protected int numTimesTriggered;
        [SerializeField] protected int triggerLimit = 1;
        
        [Space(10)]
        [SerializeField] protected FlexibleEvent triggerEvent;

        private new BoxCollider collider;

        protected bool CanInteract => triggerLimit > 0 && numTimesTriggered < triggerLimit || triggerLimit == 0;

        private void Awake()
        { 
            Rigidbody rb = GetComponent<Rigidbody>();
            collider = GetComponent<BoxCollider>();

            rb.isKinematic = true;
            numTimesTriggered = 0;
            collider.isTrigger = true;
        }

        protected virtual void OnTriggerEnter(Collider other)
        {
            if (!CanInteract) return;
            if (!other.CompareTag("Player")) return;
            
            RaiseTriggerEvent();
        }

        [ContextMenu("Raise Trigger")]
        protected void RaiseTriggerEvent()
        {
            Debug.Log("Triggered");
            
            numTimesTriggered++;
            triggerEvent.Raise();
        }

        protected virtual void OnDrawGizmos()
        {
            if (!collider) collider = GetComponent<BoxCollider>();
            
            Gizmos.color = Color.mediumSpringGreen;
            Gizmos.DrawWireCube(transform.position + collider.center, collider.size);
        }
    }
}
