namespace _Scripts.InteractionSystem
{
    public interface IInteractable
    {
        public void OnFocus();
        public void OnLostFocus();
        public void OnInteract();
    }
}
