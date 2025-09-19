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
        [SerializeField] private FlickerConfig flickerSettings;

        private float burnTime;
        private float currentBurnTime;
        private float currentIntensity;

        public bool IsLit => isLit;

        private void Awake()
        {
            light = GetComponentInChildren<Light>();

            light.range = config.range;
        }

        private void Start()
        {
            ToggleLight(false);
        }

        private void Update()
        {
            if (!isLit) return;
            
            DiminishIntensityOverTime();
            
            light.intensity = Mathf.Max(config.minIntensity, currentIntensity * flickerSettings.ApplyFlicker());
            
            if (currentBurnTime <= 0) ToggleLight(false);
        }

        private void DiminishIntensityOverTime()
        {
            if (currentBurnTime > 0) currentBurnTime -= Time.deltaTime;
            float burnProgress = 1f - (currentBurnTime / burnTime);
            float intensityMultiplier = Mathf.Lerp(1f, config.minIntensity / config.maxIntensity, burnProgress);
            currentIntensity = config.maxIntensity * intensityMultiplier;
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
