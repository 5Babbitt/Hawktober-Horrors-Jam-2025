using UnityEngine;

namespace _Scripts.Lights
{
    [CreateAssetMenu(fileName = "FlickerSettings", menuName = "Config/Lights/FlickerConfig")]
    public class FlickerConfig : ScriptableObject
    {
        [Header("Flicker Settings")]
        public float flickerSpeed = 10f;
        public float flickerIntensity = 0.1f;
        public AnimationCurve flickerCurve = AnimationCurve.EaseInOut(0, 1, 1, -1);

        private float flickerTimer = 0;
        
        public float ApplyFlicker()
        {
            flickerTimer += Time.deltaTime * flickerSpeed;
        
            // Use Perlin noise for more natural flicker
            float noiseValue = Mathf.PerlinNoise(flickerTimer, 0f);
        
            // Apply flicker curve for more control
            float curveValue = flickerCurve.Evaluate(noiseValue);
        
            // Calculate flicker multiplier (1.0 = no change, < 1.0 = dimmer, > 1.0 = brighter)
            float flickerMultiplier = 1f + ((curveValue - 0.5f) * 2f * flickerIntensity);
        
            return flickerMultiplier;
        }
    }
}