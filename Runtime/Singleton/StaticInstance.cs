using UnityEngine;

namespace JoonyleGameDevKit
{
    /// <summary>
    /// A static instance is similar to a singleton, but instead of destroying any new instances,
    /// it overrides the current instance. This is handy for resetting the state and saves you doing it manually
    /// </summary>
    public abstract class StaticInstance<T> : MonoBehaviour where T : MonoBehaviour
    {
        /// <summary>
        /// 어디에서나 접근할 수 있는 정적 인스턴스
        /// </summary>
        /// <remarks>
        /// 전역 변수가 아닌 정적 변수를 사용하여 유일성을 보장합니다.
        /// </remarks>
        public static T Instance { get; private set; }
        public static bool IsInstanceExist
        {
            get
            {
                if (Instance == null)
                {
                    // throw new System.Exception("Instance is null");
                    // Debug.LogWarning($"{Instance.GetType().Name} is null");
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        protected virtual void Awake() => Instance = this as T;

        protected virtual void OnApplicationQuit()
        {
            Instance = null;
            Destroy(this.gameObject);
        }
    }
}