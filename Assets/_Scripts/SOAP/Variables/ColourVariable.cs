using UnityEngine;

namespace _Scripts.SOAP.Variables
{
    [CreateAssetMenu(fileName = "ColourVariable", menuName = "Variables/ColourVariable")]
    public class ColourVariable : ScriptableVariable<Color>
    {
        protected override bool EqualityComparer(Color a, Color b)
        {
            return a == b;
        }
    }
}
