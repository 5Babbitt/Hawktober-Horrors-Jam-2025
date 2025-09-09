using UnityEngine.InputSystem;

namespace _Scripts.Utils
{
    public static class InputUtils
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="action"></param>
        /// <param name="callback"></param>
        /// <param name="inputPhases">Flags to decide </param>
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
        /// A method that unsubscribes all the input phases of an input action from a particular method
        /// </summary>
        /// <param name="action">The Input action whose phases are being unsubscribed</param>
        /// <param name="callback">The method being unsubscribed from</param>
        public static void UnregisterInputPhases(InputAction action, System.Action<InputAction.CallbackContext> callback)
        {
            action.canceled -= callback;
            action.performed -= callback;
            action.started -= callback;
        }
    }
    
    [System.Flags]
    public enum InputPhases
    {
        Started = 0,
        Performed = 1,
        Canceled = 2,
        All = Started | Performed | Canceled
    }
}