using UnityEngine;

namespace JoonyleGameDevKit
{
    public abstract class SingletonBehavior<T> : MonoBehaviour where T : MonoBehaviour
    {
        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<T>();

                    if (_instance == null)
                    {
                        UnityEngine.Debug.LogWarning(
                            $"No object of [ {typeof(T).Name} ] is no found \n {new System.Diagnostics.StackTrace()}");

                        /*
                         * 해당 싱글톤은 미리 생성되어 있어야 한다.
                         * 
                        GameObject singletonObject = new GameObject(typeof(T).Name);
                        _instance = singletonObject.AddComponent<T>();
                        DontDestroyOnLoad(singletonObject);
                        */
                    }
                }

                return _instance;
            }
        }

        private static T _instance = null;

        protected virtual void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(this.gameObject);
            }
            else 
            {
                _instance = this as T;
                DontDestroyOnLoad(this.gameObject);

                // Debug.Log($"[ {typeof(T).Name} ] is created");
            }
        }

        protected virtual void OnDestroy()
        {
            // Debug.Log($"Destroyed {typeof(T).Name} instance");
        }
    }
}