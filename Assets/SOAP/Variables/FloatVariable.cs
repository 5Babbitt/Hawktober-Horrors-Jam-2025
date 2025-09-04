using UnityEngine;

namespace _Scripts.SOAP.Variables
{
    [CreateAssetMenu(fileName = "FloatVariable", menuName = "Variables/FloatVariable")]
    public class FloatVariable : ScriptableVariable<float>
    {
        protected override bool EqualityComparer(float a, float b)
        {
            return Mathf.Approximately(a, b);
        }
    }
}
