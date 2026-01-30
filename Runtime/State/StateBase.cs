using UnityEngine;

namespace JoonyleGameDevKit
{
    /// <summary>
    /// 상태 인터페이스
    /// </summary>
    public interface IState<T> where T : MonoBehaviour
    {
        void Enter(T owner);
        void Exit(T owner);
        void Update(T owner);
    }

    /// <summary>
    /// 상태 기본 클래스
    /// </summary>
    public abstract class StateBase<T> : IState<T> where T : MonoBehaviour
    {
        public abstract void Enter(T owner);

        public abstract void Exit(T owner);

        public abstract void Update(T owner);
    }
}
