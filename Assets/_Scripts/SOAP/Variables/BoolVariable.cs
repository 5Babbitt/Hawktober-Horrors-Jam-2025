using UnityEngine;

namespace _Scripts.SOAP.Variables
{
    [CreateAssetMenu(fileName = "BoolVariable", menuName = "Variables/BoolVariable")]
    public class BoolVariable : ScriptableVariable<bool>
    {
        protected override bool EqualityComparer(bool a, bool b)
        {
            return a == b;
        }
    }
}
