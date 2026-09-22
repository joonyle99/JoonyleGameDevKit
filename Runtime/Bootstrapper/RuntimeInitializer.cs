using UnityEngine;

namespace JoonyleGameDevKit
{
    /// <summary>
    /// 런타임 초기화를 위한 클래스
    /// </summary>
    public static class RuntimeInitializer
    {
        private const string BOOTSTRAPPER_PREFAB_PATH = "Prefabs/Bootstrapper";

        /// <summary>
        /// 씬이 로드되기 전에 부트스트랩을 생성합니다.
        /// Bootstrapper는 선택 기능이므로, Resources/Prefabs/Bootstrapper 프리팹이
        /// 있는 프로젝트에서만 동작하고 없으면 아무것도 하지 않습니다
        /// </summary>
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void InstantiateBootstrapper()
        {
            // runtime resource load
            Bootstrapper resource = Resources.Load<Bootstrapper>(BOOTSTRAPPER_PREFAB_PATH);

            // 프리팹을 두지 않은 프로젝트는 Bootstrapper를 사용하지 않는 것으로 간주
            if (resource == null)
            {
                return;
            }

            Bootstrapper bootstrapper = Object.Instantiate(resource);
            bootstrapper.InitializeGame();
        }
    }
}
