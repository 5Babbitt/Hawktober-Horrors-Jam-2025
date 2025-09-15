using UnityEngine;

namespace _Scripts.InteractionSystem
{
    public interface IInteractable
    {
        void Focus();
        void LoseFocus();
        void InteractStart();
        void InteractCancel();
        void InteractPerform(float holdTime);
        bool CanInteract { get; }
        Transform Transform { get; }
    }
}
