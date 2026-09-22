using System;
using UnityEngine;
using System.Collections.Generic;

namespace JoonyleGameDevKit
{
    /// <summary>
    /// 플레이 모드에 진입할 때마다 정적 상태를 되돌리기 위한 등록소입니다
    /// </summary>
    /// <remarks>
    /// 1. Domain Reload를 끄면(Enter Play Mode Options) 정적 필드가 이전 플레이 세션의 값을 그대로 유지한다
    /// 2. RuntimeInitializeOnLoadMethod는 비제네릭 타입의 static 메서드만 스캔하므로 StaticInstance&lt;T&gt; 같은 제네릭 클래스에는 붙일 수 없다
    /// 3. 따라서 제네릭 쪽에서 리셋 동작을 이곳에 등록하고, 비제네릭인 이 클래스가 대신 호출한다
    /// </remarks>
    public static class SingletonReset
    {
        /// <summary>
        /// 등록 주체 타입을 키로 사용해 같은 타입이 여러 번 등록해도 누적되지 않게 합니다
        /// </summary>
        /// <remarks>
        /// 이 딕셔너리 자체는 의도적으로 비우지 않는다.
        /// 등록은 정적 생성자에서 일어나고 정적 생성자는 도메인당 한 번만 실행되므로,
        /// 여기서 비우면 다음 플레이 세션에는 되돌릴 대상이 하나도 남지 않는다.
        /// </remarks>
        private static readonly Dictionary<Type, Action> _resets = new();

        /// <summary>
        /// 플레이 모드 진입 시 실행할 리셋 동작을 등록합니다
        /// </summary>
        /// <param name="owner">정적 상태를 소유한 타입. 같은 타입으로 다시 등록하면 덮어씁니다</param>
        /// <param name="reset">되돌릴 동작</param>
        public static void Register(Type owner, Action reset)
        {
            if (owner == null || reset == null)
            {
                return;
            }

            _resets[owner] = reset;
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ResetAll()
        {
            foreach (var pair in _resets)
            {
                // 하나가 실패하더라도 나머지 정적 상태는 반드시 되돌린다
                try
                {
                    pair.Value();
                }
                catch (Exception e)
                {
                    Debug.LogError($"Failed to reset static state of {pair.Key}: {e}");
                }
            }

#if UNITY_EDITOR
            // 정적 생성자는 타입을 처음 사용할 때(Awake) 실행되고 그건 SubsystemRegistration보다 뒤다.
            // 따라서 도메인이 새로 만들어진 직후의 첫 진입은 0으로 찍히는 것이 정상이다.
            var names = new string[_resets.Count];
            var index = 0;
            foreach (var pair in _resets)
            {
                names[index++] = pair.Key.Name;
            }

            Debug.Log($"<color=cyan>Reset Statics · Singleton</color> - {_resets.Count} types\n"
                + (_resets.Count == 0
                    ? "도메인이 새로 생성되어 되돌릴 대상이 없습니다 (다음 진입부터 등록됨)"
                    : string.Join(", ", names)));
#endif
        }
    }
}
