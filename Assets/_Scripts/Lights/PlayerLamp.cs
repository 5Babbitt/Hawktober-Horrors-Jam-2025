using System.Collections;
using _Scripts.Utils;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Scripts.Lights
{
    public class PlayerLamp : MonoBehaviour
    {
        private Animator anim;
        private Light lampLight;

        [SerializeField] private bool isLit;
        [SerializeField, Range(0f, 1f)] private float lightingScale = 1;
        [SerializeField] private float currentIntensity;
        
        [Header("Light Settings")] 
        [SerializeField] private float maxIntensity = 6;
        [SerializeField] private float minIntensity = 0.5f;
        [SerializeField] private float burnTime = 20;

        [Space(10)]
        [SerializeField] private FlickerConfig flickerSettings;

        [Header("Recharge Settings")] 
        [SerializeField] private float rechargeDuration = 0.15f;
        [SerializeField] private float holdDuration = 2/3f;
        [SerializeField] private float rechargeCooldown = 1;
        [SerializeField, Range(0f, 1f)] private float maxRecharge = 0.375f;
        [SerializeField, Range(0f, 1f)] private float minRecharge = 0.3f;

        [Header("Input Settings")] 
        [SerializeField] private InputActionReference lampAction;
        
        private float flickerTimer;
        private float dimSpeed;
        private float maxIntensityDifference;
        private bool isRecharging;

        private Coroutine rechargeCoroutine;

        private void Awake()
        {
            anim = GetComponent<Animator>();
            if (!lampLight) lampLight = GetComponentInChildren<Light>();
            
            maxIntensityDifference = maxIntensity - minIntensity;
            dimSpeed = 1 / burnTime;
        }

        private void OnEnable()
        {
            lampAction.action.Enable();
            InputUtils.RegisterInputPhases(lampAction.action, OnLampInput, InputPhases.Started);
        }

        private void Update()
        {
            if (!isLit) return;

            if (!isRecharging)
            {
                DiminishIntensityOverTime();
            }

            UpdateLampIntensity();
        }

        private void UpdateLampIntensity()
        {
            lightingScale = Mathf.Clamp01(lightingScale);
            currentIntensity = minIntensity + (maxIntensityDifference * lightingScale);
            
            lampLight.intensity = currentIntensity * flickerSettings.ApplyFlicker();
        }

        private void DiminishIntensityOverTime()
        {
            lightingScale -= dimSpeed * Time.deltaTime;
        }
        
        private void OnLampInput(InputAction.CallbackContext context)
        {
            if (rechargeCoroutine != null)
            {
                return;
            }

            rechargeCoroutine = StartCoroutine(RechargeLamp());
        }
        
        private IEnumerator RechargeLamp()
        {
            float start = lightingScale;
            float target = lightingScale + Random.Range(minRecharge, maxRecharge);
            float elapsedTime = 0f;
            isRecharging = true;

            while (elapsedTime < rechargeDuration)
            {
                elapsedTime += Time.deltaTime;
                float progress = elapsedTime / rechargeDuration;

                lightingScale = Mathf.Lerp(start, target, progress);
                
                yield return null;
            }

            yield return new WaitForSeconds(holdDuration);
            
            lightingScale = target;

            yield return new WaitForSeconds(rechargeCooldown - holdDuration);
            
            isRecharging = false;
            rechargeCoroutine = null;
        }
    }
}
