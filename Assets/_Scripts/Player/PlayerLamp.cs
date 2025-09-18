using System;
using _Scripts.Utils;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Scripts.Player
{
    public class PlayerLamp : MonoBehaviour
    {
        private Animator anim;
        private Light lampLight;

        [SerializeField] private bool isLit;
        [SerializeField] private float intensity;
        
        [Header("Light Settings")] 
        [SerializeField] private  float maxIntensity;
        [SerializeField] private  float minIntensity;
        [SerializeField] private  float maxBurnTime;
        [SerializeField] private  float minBurnTime;
        
        [Header("Flicker Settings")]
        public float flickerSpeed = 10f;
        public float flickerIntensity = 0.3f;
        public AnimationCurve flickerCurve = AnimationCurve.EaseInOut(0, 1, 1, 0);

        [Header("Input Settings")] 
        [SerializeField] private InputActionReference lampAction;
        
        private float burnTime;
        private float currentBurnTime;
        private float currentIntensity;
        private float flickerTimer;

        private void Awake()
        {
            anim = GetComponent<Animator>();
            if (!lampLight) lampLight = GetComponentInChildren<Light>();
        }

        private void OnEnable()
        {
            lampAction.action.Enable();
            InputUtils.RegisterInputPhases(lampAction.action, OnLampInput, InputPhases.Started);
        }

        private void Update()
        {
            DiminishIntensityOverTime();
            
            lampLight.intensity = Mathf.Max(minIntensity, currentIntensity * ApplyFlicker());
        }

        public void Recharge()
        {
            
        }

        private void DiminishIntensityOverTime()
        {
            float burnProgress = 1f - (currentBurnTime / burnTime);
            float intensityMultiplier = Mathf.Lerp(1f, minIntensity / maxIntensity, burnProgress);
            currentIntensity = maxIntensity * intensityMultiplier;
        }
        
        private float ApplyFlicker()
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
        
        private void OnLampInput(InputAction.CallbackContext context)
        {
            Recharge();
        }
    }
}
