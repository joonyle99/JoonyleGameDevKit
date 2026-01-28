using UnityEngine;

namespace JoonyleGameDevKit
{
    /// <summary>
    /// This converts the 'static instance' into a 'basic singleton'.
    /// It will destroy any new versions created, leaving the original instance
    /// </summary>
    public abstract class Singleton<T> : StaticInstance<T> where T : MonoBehaviour
    {
        protected override void Awake()
        {
            // 이미 인스턴스가 존재하면 this 객체를 파괴하여 '싱글톤'을 보장합니다.
            if (Instance != null)
            {
                Destroy(this.gameObject);
            }

            base.Awake();
        }
    }
}
