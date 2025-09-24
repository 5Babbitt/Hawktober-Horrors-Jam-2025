using UnityEngine;

namespace _Scripts.Doors
{
    [CreateAssetMenu(fileName = "DoorAudioConfig", menuName = "Config/Audio/Door")]
    public class DoorAudioConfig : ScriptableObject
    {
        [Header("Standard Sounds")]
        public AK.Wwise.Event doorOpen;
        public AK.Wwise.Event doorClose;
        public AK.Wwise.Event doorShut;
        
        [Header("Lock Sounds")]
        public AK.Wwise.Event doorShake; // plays when trying to open a locked door
        public AK.Wwise.Event doorUnlock;
    }
}