using UnityEngine;

namespace _Scripts.Doors
{
    [CreateAssetMenu(fileName = "DoorConfig", menuName = "Config/DoorConfig")]
    public class DoorConfig : ScriptableObject
    {
        [Header("Door Settings")]
        public float closedThreshold = 2f;
        public string lockedFocusMessage = "Unlock Door";
        
        [Header("HingeJoint Settings")]
        public Vector3 anchor = Vector3.up;
        public Vector3 axis = Vector3.down;
        public bool useLimits = true;
        public float min = 0f;
        public float max = 90f;
        public float lockedMax = 1f;
        public float bounciness = 0.01f;
        public float massScale = 5f;
    }
}