using UnityEngine;

namespace _Scripts.Utils
{
    public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        public static T Instance { get; private set; }
        
        protected virtual void Awake()
        {
            if (Instance != null) 
            {
                Destroy(gameObject);
                return;
            }
            
            Instance = this as T;
        }
    }

    public abstract class PersistantSingleton<T> : Singleton<T> where T : MonoBehaviour
    {
        protected override void Awake()
        {
            base.Awake();
            
            DontDestroyOnLoad(this);
        }
    }
}