using UnityEngine;

namespace _Scripts.SOAP.Variables
{
    [CreateAssetMenu(fileName = "Vector3Variable", menuName = "Variables/Vector3Variable")]
    public class Vector3Variable : ScriptableVariable<Vector3>
    {
        protected override bool EqualityComparer(Vector3 a, Vector3 b)
        {
            return a == b;
        }
    }
}
