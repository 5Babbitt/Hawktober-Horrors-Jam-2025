using UnityEngine;

namespace _Scripts.SOAP.Variables
{
    [CreateAssetMenu(fileName = "Vector2Variable", menuName = "Variables/Vector2Variable")]
    public class Vector2Variable : ScriptableVariable<Vector2>
    {
        protected override bool EqualityComparer(Vector2 a, Vector2 b)
        {
            return a == b;
        }
    }
}
