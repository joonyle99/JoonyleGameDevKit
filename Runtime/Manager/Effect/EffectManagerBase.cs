using System;
using UnityEngine;
using UnityEngine.Pool;
using System.Collections.Generic;

namespace JoonyleGameDevKit
{
    [Serializable]
    public struct VfxEntry<TVfx> where TVfx : Enum
    {
        public TVfx type;
        public EffectBase prefab;
    }

    /// <summary>
    /// VFX 타입을 프로젝트별 enum(TVfx)으로 주입받는 이펙트 매니저 베이스.
    /// 프로젝트에서는 EffectManager : EffectManagerBase&lt;VfxType, EffectManager&gt; 형태로 상속해서 사용한다.
    /// TSelf는 Instance가 서브클래스 타입으로 반환되도록 하기 위한 CRTP 파라미터.
    /// </summary>
    public abstract class EffectManagerBase<TVfx, TSelf> : Singleton<TSelf>, IManager
        where TVfx : Enum
        where TSelf : EffectManagerBase<TVfx, TSelf>
    {
        public int Priority => 20;

        [SerializeField] private VfxEntry<TVfx>[] _vfxEntries;

        private Dictionary<TVfx, ObjectPool<EffectBase>> _pools;

        public void Initialize()
        {
            _pools = new Dictionary<TVfx, ObjectPool<EffectBase>>();

            foreach (var entry in _vfxEntries)
            {
                if (entry.prefab == null) continue;
                CreatePool(entry.type, entry.prefab);
            }
        }

        // ============== ... ==============

        private void CreatePool(TVfx type, EffectBase prefab, int defaultCapacity = 10, int maxSize = 300)
        {
            var container = new GameObject($"{type} Container").transform;
            container.SetParent(transform);

            _pools[type] = new ObjectPool<EffectBase>(
                createFunc: () => CreateEffect(type, prefab, container),
                actionOnGet: p => p.gameObject.SetActive(true),
                actionOnRelease: p => p.gameObject.SetActive(false),
                actionOnDestroy: p => { if (p != null) Destroy(p.gameObject); }, // 씬 언로드 시 컨테이너가 먼저 파괴되어 effect가 이미 destroy된 상태일 수 있음
                collectionCheck: false,
                defaultCapacity: defaultCapacity,
                maxSize: maxSize
            );
        }

        private EffectBase CreateEffect(TVfx type, EffectBase prefab, Transform container)
        {
            var effect = Instantiate(prefab, container);
            effect.name = type.ToString();
            effect.gameObject.SetActive(false);
            effect.SetReleaseAction(() => _pools[type].Release(effect));
            return effect;
        }

        // ============== ... ==============

        public void PlayVfx(TVfx type, Vector2 worldPosition, float rotation = 0f, bool flipX = false, bool flipY = false)
        {
            if (!_pools.TryGetValue(type, out var pool)) return;

            var effect = pool.Get();
            effect.Play(worldPosition, rotation, flipX, flipY);
        }

        public void PlayVfxSimulated(TVfx type, Vector2 worldPosition, float simulateTime, float rotation = 0f, bool flipX = false, bool flipY = false)
        {
            if (!_pools.TryGetValue(type, out var pool)) return;

            var effect = pool.Get();
            effect.SetSimulateTime(simulateTime);
            effect.Play(worldPosition, rotation, flipX, flipY);
        }

        // ============== ... ==============
    }
}
