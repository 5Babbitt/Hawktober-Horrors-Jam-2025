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
        }

        private void OnNotesTextChanged(string text)
        {
            noteLabel.text = text;

            notesOverlayElement.style.visibility = string.IsNullOrEmpty(text) ? Visibility.Hidden : Visibility.Visible;
        }

        private void OnInteractTextChanged(string text)
        {
            interactLabel.text = text;
        }
    }
}
