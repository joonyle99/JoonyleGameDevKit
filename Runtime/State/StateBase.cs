using UnityEngine;

namespace JoonyleGameDevKit
{
    /// <summary>
    /// 상태 기본 클래스
    /// </summary>
    public abstract class StateBase<T> where T : MonoBehaviour
    {
        public abstract void Enter(T owner);

        public abstract void Exit(T owner);

        public abstract void Update(T owner);
    }
}
