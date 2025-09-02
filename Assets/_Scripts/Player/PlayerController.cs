using System;
using UnityEngine;

namespace _Scripts.Player
{
    public class PlayerController : MonoBehaviour
    {
        public Action<bool> OnCrouch = delegate { };
    }
}
