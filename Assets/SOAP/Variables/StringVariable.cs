using UnityEngine;

namespace _Scripts.SOAP.Variables
{
    [CreateAssetMenu(fileName = "StringVariable", menuName = "Variables/StringVariable")]
    public class StringVariable : ScriptableVariable<string>
    {
        protected override bool EqualityComparer(string a, string b)
        {
            return string.Equals(a, b);
        }
    }
}