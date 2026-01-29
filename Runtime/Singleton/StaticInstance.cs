using UnityEngine;

namespace JoonyleGameDevKit
{
    /// <summary>
    /// A static instance is similar to a singleton, but instead of destroying any new instances,
    /// it overrides the current instance. This is handy for resetting the state and saves you doing it manually
    /// </summary>
    public abstract class StaticInstance<T> : MonoBehaviour where T : MonoBehaviour
    {
        protected static T _instance;
        public static T Instance
        {
            get
            {
                if (_isQuitting == true)
                {
                    return null;
                }
                
                // Lazy initialization: 인스턴스가 없을 때만 생성합니다
                if (_instance == null)
                {
                    // 씬에서 이미 존재하는 인스턴스를 먼저 찾습니다
                    _instance = FindAnyObjectByType<T>();

                    if (_instance == null)
                    {
                        // 씬에 없으면 새 GameObject를 생성하여 인스턴스 추가합니다
                        var gameObject = new GameObject();
                        gameObject.name = typeof(T).Name;
                        _instance = gameObject.AddComponent<T>();
                    }
                }
                
                return _instance;
            }
        }

        private static bool _isQuitting;

        protected virtual void Awake()
        {
            _instance = this as T;
        }
        protected virtual void OnDestroy()
        {
            // 내가 진짜 싱글톤 인스턴스일 때만 정리합니다
            if (_instance == this)
            {
                _instance = null;
            }
        }

        protected virtual void OnApplicationQuit()
        {
            _instance = null;
            _isQuitting = true;
        }
    }
}