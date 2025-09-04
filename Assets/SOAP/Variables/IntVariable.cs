using UnityEngine;

namespace _Scripts.SOAP.Variables
{
    [CreateAssetMenu(fileName = "IntVariable", menuName = "Variables/IntVariable")]
    public class IntVariable : ScriptableVariable<int>
    {
        protected override bool EqualityComparer(int a, int b)
        {
            return a == b;
        }
    }
}
