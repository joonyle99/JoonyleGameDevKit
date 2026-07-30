using System;
using UnityEngine;

namespace JoonyleGameDevKit
{
    /// <summary>
    /// 상태 전이 정보를 담는 클래스
    /// </summary>
    public class Transition<T> where T : MonoBehaviour
    {
        /// <summary>
        /// 전이 시작 상태
        /// </summary>
        private StateBase<T> _from;
        public StateBase<T> From => _from;

        /// <summary>
        /// 전이 도착 상태
        /// </summary>
        private StateBase<T> _to;
        public StateBase<T> To => _to;

        /// <summary>
        /// 전이 조건
        /// </summary>
        private Func<bool> _condition;
        public Func<bool> Condition => _condition;

        public Transition(StateBase<T> from, StateBase<T> to, Func<bool> condition)
        {
            _from = from;
            _to = to;
            _condition = condition;
        }
    }
}
