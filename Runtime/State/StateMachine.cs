using System;
using UnityEngine;
using System.Collections.Generic;

namespace JoonyleGameDevKit
{
    // ============================================
    // 상태 패턴 기반 FSM의 한계점
    // ============================================
    //
    // [한계점]
    // 1. 애니메이션 블랜딩 대처가 어려움
    // 2. 상태 수 증가 시 복잡도 급증
    //    - 전이 개수: 최대 N(N-1)개 (N = 상태 수)
    //    - 전이 조건 관리가 기하급수적으로 어려워짐 (&&와 ||가 덕지 덕지)
    //
    // [권장 사용 사례]
    // - FSM 적합: 5개 미만의 상태, 플레이어처럼 입력 기반 (전이 조건이 단순)
    // - BT 적합: 몬스터 AI처럼 복잡한 전이 조건이 필요한 경우 → Behaviour Tree 권장
    // ============================================

    /// <summary>
    /// 유한 상태 머신 (FSM)
    /// </summary>
    public class StateMachine<T> where T : MonoBehaviour
    {
        public StateMachine(T owner)
        {
            _owner = owner;
        }

        /// <summary>
        /// 상태 머신 소유자
        /// </summary>
        private T _owner;

        /// <summary>
        /// 등록된 상태 목록
        /// </summary>
        private Dictionary<Type, StateBase<T>> _states = new();

        /// <summary>
        /// 등록된 전이 목록
        /// </summary>
        private List<Transition<T>> _transitions = new();

        /// <summary>
        /// 현재 상태
        /// </summary>
        private StateBase<T> _currState;
        public StateBase<T> CurrState => _currState;

        /// <summary>
        /// 상태를 인스턴스화
        /// </summary>
        public void AddState(StateBase<T> state)
        {
            _states[state.GetType()] = state;
        }

        /// <summary>
        /// AddState로 상태를 먼저 추가합니다
        /// </summary>
        public void AddTransition<TStateFrom, TStateTo>(Func<bool> condition)
            where TStateFrom : StateBase<T>
            where TStateTo : StateBase<T>
        {
            var hasFrom = _states.TryGetValue(typeof(TStateFrom), out StateBase<T> from);
            var hasTo = _states.TryGetValue(typeof(TStateTo), out StateBase<T> to);

            if (hasFrom == false || hasTo == false)
            {
                Debug.LogWarning($"AddTransition 실패: {typeof(TStateFrom).Name} 또는 {typeof(TStateTo).Name} 상태가 등록되지 않았습니다.");
                return;
            }

            var transition = new Transition<T>(from, to, condition);
            _transitions.Add(transition);
        }

        /// <summary>
        /// 사용자가 직접 호출할 때 사용합니다 (컴파일 타임)
        /// </summary>
        /// <remarks>
        /// 제네릭 타입을 사용
        /// </remarks>
        public void ChangeState<TState>() where TState : StateBase<T>
        {
            var hasState = _states.TryGetValue(typeof(TState), out StateBase<T> targetState);
            if (hasState == false) return;

            _currState?.Exit(_owner);
            _currState = targetState;
            _currState?.Enter(_owner);
        }

        /// <summary>
        /// FSM 내부에서 Transition 처리할 때 사용합니다 (런타임)
        /// </summary>
        /// <remarks>
        /// 상태 인스턴스 사용
        /// </remarks>
        public void ChangeState(StateBase<T> state)
        {
            var hasState = _states.TryGetValue(state.GetType(), out StateBase<T> targetState);
            if (hasState == false) return;

            _currState?.Exit(_owner);
            _currState = targetState;
            _currState?.Enter(_owner);
        }

        public void Update()
        {
            // 전이 조건부터 체크한다
            foreach (var transition in _transitions)
            {
                var from = transition.From;
                if (_currState == from)
                {
                    var condition = transition.Condition.Invoke();
                    if (condition == true)
                    {
                        var to = transition.To;
                        ChangeState(to);
                        break;
                    }
                }
            }
            
            _currState?.Update(_owner);
        }
    }
}
