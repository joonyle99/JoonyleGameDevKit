using UnityEngine;

namespace JoonyleGameDevKit
{
    /// <summary>
    /// This will survive through scene loads.
    /// Perfect for system classes which require stateful, persistent data,
    /// audio sources where music plays through loading screens, etc
    /// </summary>
    public abstract class PersistentSingleton<T> : Singleton<T> where T : MonoBehaviour
    {
        protected override void Awake()
        {
            base.Awake();

            // 해당 싱글톤 객체를 씬이 변경되어도 파괴되지 않도록 설정합니다
            DontDestroyOnLoad(this.gameObject);
        }
    }
}
