using _Scripts.InteractionSystem;
using _Scripts.SOAP.Variables;
using UnityEngine;

namespace _Scripts.Notes
{
    [RequireComponent(typeof(BoxCollider))]
    public class NotesInteractable : InteractableBehaviour
    {
        [SerializeField] private NotesConfig config;
        [SerializeField] private StringVariable noteUIText;
        [SerializeField] private string noteKey;
        
        private MeshRenderer meshRenderer;
        private Material materialInstance;
        
        private void Awake()
        {
            meshRenderer = GetComponent<MeshRenderer>();
            materialInstance = new Material(meshRenderer.material);
            meshRenderer.material = materialInstance;
            Color finalEmissionColor = config.emissionColor * config.emissionIntensity;
            materialInstance.SetColor("_EmissionColor", finalEmissionColor);
        }

        protected override void OnFocus()
        {
            // Add highlight
            materialInstance.EnableKeyword("_EMISSION");
        }

        protected override void OnLoseFocus()
        {
            // Remove highlight
            materialInstance.DisableKeyword("_EMISSION");
        }

        protected override void OnInteractStart()
        {
            noteUIText.Value = NoteSystem.Instance.GetNote(noteKey);
            config.toggleNotesUI.Raise(true);
        }

        protected override void OnInteractCanceled()
        {
            config.toggleNotesUI.Raise(false);
            noteUIText.Value = string.Empty;
        }

        protected override void OnInteractPerformed(float holdTime) { }
    }
}
