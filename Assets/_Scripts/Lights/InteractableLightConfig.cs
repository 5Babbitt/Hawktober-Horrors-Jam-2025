using UnityEngine;

namespace _Scripts.Lights
{
    [CreateAssetMenu(fileName = "LightConfig", menuName = "Config/LightConfig")]
    public class InteractableLightConfig : ScriptableObject
    {
        [Header("Interactable Settings")] 
        public string altFocusText = "Extinguish";
        
        [Header("Light Settings")] 
        public float maxIntensity;
        public float minIntensity;
        public float maxBurnTime;
        public float minBurnTime;
        
        [Header("Flicker Settings")]
        public float flickerSpeed = 10f;
        public float flickerIntensity = 0.3f;
        public AnimationCurve flickerCurve = AnimationCurve.EaseInOut(0, 1, 1, 0);
    }
}