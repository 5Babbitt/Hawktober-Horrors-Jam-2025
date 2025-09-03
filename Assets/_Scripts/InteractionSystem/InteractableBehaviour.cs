using UnityEngine;

namespace _Scripts.InteractionSystem
{
    public abstract class InteractableBehaviour : MonoBehaviour, IInteractable
    {
        public virtual void OnFocus()
        {
            Debug.Log($"Looking at {name}");
        }

        public virtual void OnLostFocus()
        {
            
        }

        public virtual void OnInteract()
        {
            Debug.Log($"Interacted with {name}");
        }
    }

    public class InteractableCube : InteractableBehaviour
    {
        
    }
}