using System;
using UnityEngine;

namespace JoonyleGameDevKit
{
    /// <summary>
    /// enum 기반의 가벼운 게임 상태 컨트롤러.
    /// 상태별 클래스가 필요 없는 씬 단위 흐름(로비/플레이/결과 등)에 적합하다.
    /// 상태 패턴이 필요하면 StateMachine{T}를 사용한다.
    /// </summary>
    public class GameStateController<T> where T : Enum
    {
        private T currState;
        public T CurrState => currState;

        public event Action<T, T> OnStateChanged;

        public void ChangeState(T nextState)
        {
            if (currState.Equals(nextState)) return;

            var prevState = currState;
            ExitState(currState);
            currState = nextState;
            EnterState(currState);

            OnStateChanged?.Invoke(prevState, currState);
        }

        private void EnterState(T state)
        {
#if UNITY_EDITOR
            Debug.Log($"<color=yellow>Enter State - {state.ToString()}</color>");
#endif
        }

        private void ExitState(T state)
        {
#if UNITY_EDITOR
            Debug.Log($"<color=yellow>Exit State - {state.ToString()}</color>");
#endif
        }
    }
}
