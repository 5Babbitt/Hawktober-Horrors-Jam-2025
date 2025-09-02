using System;
using UnityEngine;

namespace _Scripts.Player
{
    public abstract class PlayerFeature : MonoBehaviour
    {
        protected PlayerController Controller;

        protected virtual void Awake()
        {
            Controller = transform.root.GetComponent<PlayerController>();
        }
    }
}
