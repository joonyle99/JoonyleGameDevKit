using System;

namespace JoonyleGameDevKit
{
    /// <summary>
    /// GameStateController{T}의 상태 변경을 구독하는 리스너.
    /// controller.OnStateChanged += listener.OnStateChanged 형태로 연결한다.
    /// </summary>
    public interface IGameStateListener<T> where T : Enum
    {
        void OnStateChanged(T prevState, T currState);
    }
}
