using UnityEngine;

namespace _Scripts.InteractionSystem.Interactables
{
    public class InteractableCube : InteractableBehaviour
    {
        private InteractableBehaviour interactable;
    
        private void Awake()
        {
            interactable = GetComponent<InteractableBehaviour>();
        }
    
        private void OnEnable()
        {
            interactable.onInteractEvent.AddListener(GrowCube);
        }
    
        private void OnDisable()
        {
            interactable.onInteractEvent.RemoveListener(GrowCube);
        }

        public override bool OnInteract()
        {
            if (!base.OnInteract()) return false;
            GrowCube();

            return true;
        }

        public void GrowCube()
        {
            transform.localScale *= 1.1f;
        }
    }
}
