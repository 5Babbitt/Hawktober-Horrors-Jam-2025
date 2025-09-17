using System;
using _Scripts.InteractionSystem;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Scripts.Lights
{
    public class InteractableLight : InteractableBehaviour
    {
        [Header("Light Settings")] 
        [SerializeField] private bool isLit;
        [SerializeField] private new Light light;
        [SerializeField] private InteractableLightConfig config;

        private float burnTime;
        private float currentBurnTime;
        private float currentIntensity;
        private float flickerTimer;

        public bool IsLit => isLit;

        private void Awake()
        {
            light = GetComponentInChildren<Light>();
        }

        private void Start()
        {
            ToggleLight(false);
        }

        private void Update()
        {
            if (!isLit) return;

            currentBurnTime -= Time.deltaTime;

            DiminishIntensityOverTime();
            
            light.intensity = Mathf.Max(config.minIntensity, currentIntensity * ApplyFlicker());
            
            if (currentBurnTime <= 0) ToggleLight(false);
        }

        private void DiminishIntensityOverTime()
        {
            float burnProgress = 1f - (currentBurnTime / burnTime);
            float intensityMultiplier = Mathf.Lerp(1f, config.minIntensity / config.maxIntensity, burnProgress);
            currentIntensity = config.maxIntensity * intensityMultiplier;
        }
        
        private float ApplyFlicker()
        {
            flickerTimer += Time.deltaTime * config.flickerSpeed;
        
            // Use Perlin noise for more natural flicker
            float noiseValue = Mathf.PerlinNoise(flickerTimer, 0f);
        
            // Apply flicker curve for more control
            float curveValue = config.flickerCurve.Evaluate(noiseValue);
        
            // Calculate flicker multiplier (1.0 = no change, < 1.0 = dimmer, > 1.0 = brighter)
            float flickerMultiplier = 1f + ((curveValue - 0.5f) * 2f * config.flickerIntensity);
        
            return flickerMultiplier;
        }

        public void ExtinguishLight()
        {
            ToggleLight(false);
        }

        protected override void OnFocus()
        {
            SetInteractUIText(isLit ? config.altFocusText : focusText);
        }

        protected override void OnLoseFocus() { }

        protected override void OnInteractStart()
        {
            ToggleLight();
        }

        protected override void OnInteractCanceled() { }

        protected override void OnInteractPerformed(float holdTime) { }

        private void ToggleLight()
        {
            ToggleLight(!isLit);
        }
        
        private void ToggleLight(bool value)
        {
            isLit = value;

            if (isLit)
            {
                burnTime = Random.Range(config.minBurnTime, config.maxBurnTime);
                flickerTimer = Random.Range(0f, 100f); 
                currentIntensity = config.maxIntensity;
                currentBurnTime = burnTime;
            }
            else
            {
                burnTime = 0;
                currentIntensity = 0;
                currentBurnTime = 0;
            }
            
            light.intensity = currentIntensity;
        }
    }
}
