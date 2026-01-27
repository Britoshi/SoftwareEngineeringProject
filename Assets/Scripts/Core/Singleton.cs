using UnityEngine;

namespace Core
{
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        internal static T Instance { get; set; }
        protected virtual void Awake()
        {
            if (Instance != null)
            {
                throw new System.Exception("Attempting to instantiate multiple instances of singleton");
            }
            Instance = this as T;
        }
    }
}
