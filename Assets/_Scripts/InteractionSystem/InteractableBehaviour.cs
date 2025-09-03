using UnityEngine;

namespace _Scripts.InteractionSystem
{
    public abstract class InteractableBehaviour : MonoBehaviour, IInteractable
    {
        public BoxCollider interactionSpace;
    }
}