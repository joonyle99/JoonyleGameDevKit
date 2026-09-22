using System;
using UnityEngine;

namespace JoonyleGameDevKit
{
    /// <summary>
    /// RuntimeInitializer에 의해 씬이 로드되기 전에 생성되며,
    /// 게임 시작 전에 필요한 매니저를 초기화합니다.
    /// 선택 기능이므로, 사용하려는 프로젝트만 Resources/Prefabs/Bootstrapper 프리팹을 두면 됩니다
    /// (Bootstrapper 샘플을 임포트하면 바로 사용 가능)
    /// </summary>
    public class Bootstrapper : PersistentSingleton<Bootstrapper>
    {
        public void InitializeGame()
        {
            Debug.Log($"<color=green>Initialize Game</color>");

            // FPS를 60으로 고정하여 프레임 튀는 것을 방지
            Application.targetFrameRate = 60;

            // 매니저 초기화
            var managers = GetComponentsInChildren<IManager>();
            Array.Sort(managers, (a, b) => a.Priority.CompareTo(b.Priority));
            Array.ForEach(managers, m => m.Initialize());
        }
    }
}
