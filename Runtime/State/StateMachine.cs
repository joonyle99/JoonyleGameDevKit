using System;
using UnityEngine;
using System.Collections.Generic;

namespace JoonyleGameDevKit
{
    /// <summary>
    /// 유한 상태 머신 (FSM)
    /// </summary>
    /// <typeparam name="T">상태 머신을 소유하는 MonoBehaviour 타입</typeparam>
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
        /// 현재 상태
        /// </summary>
        private StateBase<T> _currState;
        public StateBase<T> CurrState => _currState;

        /// <summary>
        /// 상태를 등록합니다.
        /// </summary>
        /// <param name="state">등록할 상태</param>
        public void AddState(StateBase<T> state)
        {
            _states[state.GetType()] = state;
        }

        /// <summary>
        /// 상태를 전환합니다.
        /// </summary>
        /// <typeparam name="TState">전환할 상태 타입</typeparam>
        public void ChangeState<TState>() where TState : StateBase<T>
        {
            var hasState = _states.TryGetValue(typeof(TState), out StateBase<T> targetState);
            if (hasState == false) return;

            _currState?.Exit(_owner);
            _currState = targetState;
            _currState?.Enter(_owner);
        }

        /// <summary>
        /// 현재 상태의 Update를 호출합니다.
        /// </summary>
        public void Update()
        {
            _currState?.Update(_owner);
        }
    }
}
