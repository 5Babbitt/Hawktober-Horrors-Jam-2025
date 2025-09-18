using System;
using _Scripts.InteractionSystem;
using _Scripts.InventorySystem;
using _Scripts.SOAP.EventSystem;
using _Scripts.SOAP.EventSystem.Events;
using _Scripts.SOAP.Variables;
using UnityEngine;

namespace _Scripts.PickupInteractables
{
    [RequireComponent(typeof(BoxCollider), typeof(Rigidbody))]
    public class PickupInteractable : InteractableBehaviour
    {
        [Header("Pickup Settings")]
        [SerializeField] private StringVariable itemKey;
        [SerializeField] private StringEvent onPickupItem;

        private MeshRenderer meshRenderer;
        private Material materialInstance;
        
        [Header("Emission Settings")]
        [SerializeField] private Color emissionColor = Color.yellow;
        [SerializeField] private float emissionIntensity = 0.5f;

        private void Awake()
        {
            meshRenderer = GetComponent<MeshRenderer>();
            materialInstance = new Material(meshRenderer.material);
            meshRenderer.material = materialInstance;
            Color finalEmissionColor = emissionColor * emissionIntensity;
            materialInstance.SetColor("_EmissionColor", finalEmissionColor);
        }

        protected override void OnFocus()
        {
            // Add highlight
            materialInstance.EnableKeyword("_EMISSION");
        }

        protected override void OnLoseFocus()
        {
            // Remove highlight
            materialInstance.DisableKeyword("_EMISSION");
        }

        protected override void OnInteractStart()
        {
            if (string.IsNullOrEmpty(itemKey.Value))
            {
                Debug.LogError("Item Key Invalid");
                return;
            }
            
            if (Inventory.Instance.HasItem(itemKey.Value)) return;
            Inventory.Instance.AddItem(itemKey.Value);
            onPickupItem?.Raise(itemKey.Value);
        }

        protected override void OnInteractCanceled()
        {
            gameObject.SetActive(false);
        }

        protected override void OnInteractPerformed(float holdTime) { }
    }
}