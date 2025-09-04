using UnityEngine;

namespace _Scripts.Utils
{
    public class Bootstrapper : PersistantSingleton<Bootstrapper>
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void Init()
        {
            DontDestroyOnLoad(Instantiate(Resources.Load("Systems")));
        }
    }
}

