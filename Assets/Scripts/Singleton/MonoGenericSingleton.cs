using UnityEngine;
namespace Singleton
{
    public class MonoGenericSingleton<T> : MonoBehaviour where T: MonoGenericSingleton<T>
    {
        public static T Instance { get; private set; }

        protected virtual void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = (T) this;
                DontDestroyOnLoad(gameObject);
            }
        }
    }
}