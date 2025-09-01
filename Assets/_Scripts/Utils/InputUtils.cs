using UnityEngine.InputSystem;

namespace _Scripts.Utils
{
    public static class InputUtils
    {
        public static void RegisterInputPhases(InputAction action, System.Action<InputAction.CallbackContext> callback)
        {
            action.canceled += callback;
            action.performed += callback;
            action.started += callback;
        }
        
        public static void UnregisterInputPhases(InputAction action, System.Action<InputAction.CallbackContext> callback)
        {
            action.canceled -= callback;
            action.performed -= callback;
            action.started -= callback;
        }
    }
}