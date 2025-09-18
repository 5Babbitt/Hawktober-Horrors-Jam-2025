using _Scripts.SOAP.EventSystem.Events;
using _Scripts.SOAP.Variables;
using UnityEngine;

namespace _Scripts.Notes
{
    [CreateAssetMenu(fileName = "NotesConfig", menuName = "Config/NotesConfig")]
    public class NotesConfig : ScriptableObject
    {
        public BoolEvent toggleNotesUI;
        
        [Header("Emission Settings")]
        public Color emissionColor = Color.yellow;
        public float emissionIntensity = 0.5f;
    }
}