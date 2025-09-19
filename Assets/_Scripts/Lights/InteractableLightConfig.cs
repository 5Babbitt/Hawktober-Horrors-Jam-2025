using UnityEngine;

namespace _Scripts.Lights
{
    [CreateAssetMenu(fileName = "LightConfig", menuName = "Config/Lights/LightConfig")]
    public class InteractableLightConfig : ScriptableObject
    {
        [Header("Interactable Settings")] 
        public string altFocusText = "Extinguish";

        [Header("Light Settings")] 
        public float range = 20f;
        public float maxIntensity = 10f;
        public float minIntensity = 0.1f;
        public float maxBurnTime = 120f;
        public float minBurnTime = 60f;
    }
}