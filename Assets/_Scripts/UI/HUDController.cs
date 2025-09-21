using System;
using _Scripts.SOAP.Variables;
using UnityEngine;
using UnityEngine.UIElements;

namespace _Scripts.UI
{
    public class HUDController : MonoBehaviour
    {
        private const string HudElementName = "HUD";
        private const string InteractLabelName = "InteractText";
        private const string NoteLabelName = "NotesText";
        private const string NotesOverlayName = "NotesOverlay";
        
        public StringVariable interactUIText;
        public StringVariable notesUIText;
        public UIDocument doc;

        private Label interactLabel;
        private Label noteLabel;

        private VisualElement hudElement;
        private VisualElement notesOverlayElement;

        [SerializeField] private Color notesTint;
        [SerializeField] private Color notesTintDarkest;
        
        private void Awake()
        {
            var root = doc.rootVisualElement;

            hudElement = root.Q<VisualElement>(HudElementName);
            notesOverlayElement = root.Q<VisualElement>(NotesOverlayName);
            
            interactLabel = root.Q<Label>(InteractLabelName);
            noteLabel = root.Q<Label>(NoteLabelName);
        }

        private void OnEnable()
        {
            interactUIText.OnValueChanged += OnInteractTextChanged;
            notesUIText.OnValueChanged += OnNotesTextChanged;
        }
        
        private void OnDisable()
        {
            interactUIText.OnValueChanged -= OnInteractTextChanged;
            notesUIText.OnValueChanged -= OnNotesTextChanged;
        }

        private void Start()
        {
            interactUIText.Value = "";
            notesUIText.Value = "";

            notesTint = notesOverlayElement.resolvedStyle.unityBackgroundImageTintColor;
        }

        private void OnNotesTextChanged(string text)
        {
            noteLabel.text = text;
            UpdateNotesTint();

            notesOverlayElement.style.visibility = string.IsNullOrEmpty(text) ? Visibility.Hidden : Visibility.Visible;
        }

        private void SetNotesTintColor(float brightness)
        {
            
        }

        private void OnInteractTextChanged(string text)
        {
            interactLabel.text = text;
        }
        
        private float GetLightingBrightness()
        {
            var lights = FindObjectsByType<Light>(FindObjectsSortMode.None);
            float brightness = RenderSettings.ambientLight.grayscale;
    
            foreach (var lightUnit in lights)
            {
                if (!lightUnit.enabled) continue;
                var distance = Vector3.Distance(transform.position, lightUnit.transform.position);
                if (distance < lightUnit.range)
                {
                    var attenuation = 1f - (distance / lightUnit.range);
                    brightness += lightUnit.intensity * lightUnit.color.grayscale * attenuation;
                }
            }
    
            return Mathf.Clamp01(brightness - 0.5f);
        }
        
        private void UpdateNotesTint()
        {
            float brightness = GetLightingBrightness(); // or whichever method you choose
    
            Debug.Log(brightness);
            
            // Darker environments make notes harder to read
            Color adjustedTint = Color.Lerp(notesTintDarkest, notesTint, brightness);
            notesOverlayElement.style.unityBackgroundImageTintColor = adjustedTint;
        }
    }
}
