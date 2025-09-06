using UnityEngine;

namespace _Scripts.InteractionSystem
{
    public class InteractableCube : MonoBehaviour
    {
        public void GrowCube()
        {
            transform.localScale *= 1.1f;
        }
    }
}