using UnityEngine;

namespace _Scripts.Audio
{
    public class SoundMaterial : MonoBehaviour
    {
        [field: SerializeField] public AK.Wwise.Switch Material { get; private set; }
    }
}
