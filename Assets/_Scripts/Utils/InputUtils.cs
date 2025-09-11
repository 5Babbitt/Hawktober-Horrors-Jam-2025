using UnityEngine.InputSystem;

namespace _Scripts.Utils
{
    public static class InputUtils
    {
        /// <summary>
        /// A method that subscribes the input phases of an input action to a particular method
        /// </summary>
        /// <param name="action"></param>
        /// <param name="callback"></param>
        /// <param name="inputPhases">Flags to decide which input phases to register (use only if you want to exclude specific phases)</param>
        public static void RegisterInputPhases(InputAction action, System.Action<InputAction.CallbackContext> callback, InputPhases inputPhases = InputPhases.All)
        {
            if (inputPhases.HasFlag(InputPhases.Started))
                action.started += callback;
            if (inputPhases.HasFlag(InputPhases.Performed))
                action.performed += callback;
            if (inputPhases.HasFlag(InputPhases.Canceled))
                action.canceled += callback;
        }
        
        /// <summary>
        /// A method that unsubscribes the input phases of an input action from a particular method
        /// </summary>
        /// <param name="action">The Input action whose phases are being unsubscribed</param>
        /// <param name="callback">The method being unsubscribed from</param>
        /// <param name="inputPhases">Flags to decide which input phases to unregister (use only if you want to exclude specific phases)</param>
        public static void UnregisterInputPhases(InputAction action, System.Action<InputAction.CallbackContext> callback, InputPhases inputPhases = InputPhases.All)
        {
            if (inputPhases.HasFlag(InputPhases.Started))
                action.started -= callback;
            if (inputPhases.HasFlag(InputPhases.Performed))
                action.performed -= callback;
            if (inputPhases.HasFlag(InputPhases.Canceled))
                action.canceled -= callback;
        }
    }
    
    [System.Flags]
    public enum InputPhases
    {
        Started = 1,
        Performed = 2,
        Canceled = 4,
        All = Started | Performed | Canceled
    }
}