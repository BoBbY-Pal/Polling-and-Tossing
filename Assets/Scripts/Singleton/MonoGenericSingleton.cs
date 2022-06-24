using UnityEngine;
namespace Singleton
{
    public class MonoGenericSingleton<T> : MonoBehaviour where T: MonoGenericSingleton<T>
    {
        private static T _instance;
        public static T Instance => _instance;

        protected virtual void Awake()
        {
            if (_instance != null)
            {
                Destroy(gameObject);
            }
            else
            {
                _instance = (T) this;
                DontDestroyOnLoad(gameObject);
            }
        }
    }
}