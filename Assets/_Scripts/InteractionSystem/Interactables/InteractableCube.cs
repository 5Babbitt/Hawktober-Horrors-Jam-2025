namespace _Scripts.InteractionSystem.Interactables
{
    public class InteractableCube : InteractableBehaviour
    {
        private void GrowCube()
        {
            transform.localScale *= 1.1f;
        }

        protected override void OnFocus() { }

        protected override void OnLoseFocus() { }

        protected override void OnInteractStart()
        {
            GrowCube();
        }

        protected override void OnInteractCanceled() { }

        protected override void OnInteractPerformed(float holdTime) { }
    }
}
