using System;
using UnityEngine;
using System.Collections.Generic;
#if UNITY_6000_5_OR_NEWER
using SpriteId = UnityEngine.EntityId;
#else
using SpriteId = System.Int32;
#endif

namespace JoonyleGameDevKit
{
    /// <summary>
    /// 파편이 사라지는 방식을 정의합니다
    /// </summary>
    [Serializable]
    public struct FadeOption
    {
        public bool UseFade;
        public float FadeDelay;
        public float FadeDuration;

        /// <summary>
        /// timeScale의 영향을 받지 않고 실시간으로 페이드할지 여부입니다
        /// </summary>
        /// <remarks>
        /// 기본값은 false(scaled)다. 파편은 Rigidbody2D로 날아가고 물리는 timeScale을 따르므로,
        /// 페이드만 실시간으로 두면 배속/슬로우 구간에서 움직임과 소멸이 어긋난다.
        /// 일시정지 중에도 파편이 정리되어야 하는 경우에만 켠다.
        /// </remarks>
        public bool UseUnscaledTime;

        public static FadeOption None => new FadeOption
        {
            UseFade = false,
            FadeDelay = 0f,
            FadeDuration = 0f,
            UseUnscaledTime = false
        };

        public static FadeOption Default => new FadeOption
        {
            UseFade = true,
            FadeDelay = 5f,
            FadeDuration = 0.5f,
            UseUnscaledTime = false
        };
    }

    /// <summary>
    /// 스프라이트 폭발 연출의 모든 파라미터입니다
    /// </summary>
    [Serializable]
    public struct ExplodeOption
    {
        /// <summary><see cref="SortingOrderOverride"/>를 쓰지 않는다는 표시입니다</summary>
        public const int NO_SORTING_ORDER_OVERRIDE = int.MinValue;

        [Header("Slice")]
        public int SliceCount;
        public bool HasCollider;
        public string PieceLayerName;

        [Header("Force")]
        public float Force;
        public float GravityScale;
        public Vector2 ForceRangeX;
        public Vector2 ForceRangeY;
        public float TorqueRange;

        [Header("Render")]
        public int SortingOrderOffset;

        /// <summary>
        /// 파편 조각이 쓸 머티리얼입니다. null이면 SpriteRenderer의 기본 머티리얼을 그대로 둡니다
        /// </summary>
        /// <remarks>
        /// 조명을 쓰지 않는 프로젝트에서 Lit 머티리얼을 그대로 두면 파편이 조명 경로를 타면서
        /// 동적 배칭이 되지 않아 조각 하나당 드로우콜 하나가 나간다.
        /// 그런 프로젝트는 여기에 Unlit 머티리얼을 넘겨 조각들이 한 배치로 묶이게 한다.
        /// </remarks>
        public Material PieceMaterial;

        /// <summary>원본 renderer의 order를 무시하고 이 값을 그대로 씁니다. <see cref="NO_SORTING_ORDER_OVERRIDE"/>면 사용하지 않습니다</summary>
        public int SortingOrderOverride;

        [Header("Lifetime")]
        public FadeOption Fade;

        /// <summary>
        /// 페이드를 쓰지 않을 때 파편을 강제로 회수하기까지의 시간입니다. 0 이하면 회수하지 않습니다
        /// </summary>
        /// <remarks>
        /// 페이드가 꺼져 있으면 파편을 되돌릴 근거가 하나도 없어 풀이 고갈될 때까지 화면에 남는다.
        /// 회수 기한을 하나 두어 "언젠가는 반드시 돌아온다"를 보장한다.
        /// </remarks>
        public float MaxLifetime;

        /// <summary>
        /// 이 속도 아래로 <see cref="SettleHold"/>만큼 유지되면 안착한 것으로 보고 물리에서 뺍니다. 0 이하면 빼지 않습니다
        /// </summary>
        /// <remarks>
        /// 파편이 바닥에 쌓여 서로 맞닿은 채로 남아 있는 것이 물리 비용의 대부분이다.
        /// 안착한 파편은 더 움직일 일이 없으므로 그 자리에 세워 두고 시뮬레이션에서 빼면
        /// 보이는 모습은 그대로면서 비용만 사라진다.
        /// </remarks>
        public float SettleSpeed;

        /// <summary>
        /// <see cref="SettleSpeed"/> 아래 속도가 이만큼 이어져야 안착으로 인정합니다
        /// </summary>
        /// <remarks>
        /// 포물선의 꼭대기에서도 속도는 순간적으로 0에 가까워진다.
        /// 그때 얼려 버리면 파편이 공중에 박히므로, 잠깐이 아니라 계속 느린 경우만 안착으로 본다.
        /// </remarks>
        public float SettleHold;

        /// <param name="pieceLayerName">
        /// 파편을 올릴 레이어. 프로젝트마다 이름이 다르므로 기본값을 두지 않고 반드시 받는다.
        /// 기본값을 두면 그 레이어가 없는 프로젝트에서 파편이 조용히 Default 레이어로 떨어져,
        /// 파편끼리 충돌하고 플레이어를 밀어내는데 원인은 드러나지 않는다.
        /// </param>
        public static ExplodeOption Default(string pieceLayerName) => new ExplodeOption
        {
            SliceCount = 4,
            HasCollider = true,
            PieceLayerName = pieceLayerName,
            Force = 50f,
            GravityScale = 1f,
            ForceRangeX = new Vector2(-1f, 1f),
            ForceRangeY = new Vector2(0.5f, 1f),
            TorqueRange = 200f,
            SortingOrderOffset = 0,
            SortingOrderOverride = NO_SORTING_ORDER_OVERRIDE,
            Fade = FadeOption.Default,
            MaxLifetime = 10f,
            SettleSpeed = 0.4f,
            SettleHold = 0.2f
        };

        /// <summary>힘이 빠지며 부서지는 연출 - 약한 힘, 위로 떠오름, 빠른 페이드 아웃</summary>
        /// <param name="pieceLayerName">파편을 올릴 레이어. <see cref="Default"/>와 같은 이유로 기본값을 두지 않는다</param>
        public static ExplodeOption Dissolve(string pieceLayerName) => new ExplodeOption
        {
            SliceCount = 4,
            HasCollider = false,
            PieceLayerName = pieceLayerName,
            Force = 50f,
            GravityScale = -0.1f,
            ForceRangeX = new Vector2(-0.5f, 0.5f),
            ForceRangeY = new Vector2(0.3f, 1f),
            TorqueRange = 50f,
            SortingOrderOffset = 1,
            SortingOrderOverride = NO_SORTING_ORDER_OVERRIDE,
            Fade = new FadeOption { UseFade = true, FadeDelay = 0.3f, FadeDuration = 0.6f },
            MaxLifetime = 5f,
            SettleSpeed = 0f,
            SettleHold = 0f
        };
    }

    /// <summary>
    /// 스프라이트를 N×N 조각으로 분할하여 폭발시키는 유틸리티입니다
    /// </summary>
    /// <remarks>
    /// 조각 GameObject와 분할된 Sprite를 모두 <see cref="SpriteExplosionPool"/>에서 재사용하므로
    /// 폭발이 반복되어도 새로 할당하지 않는다.
    /// </remarks>
    public static class SpriteExploder
    {
        /// <summary>
        /// 스프라이트를 조각으로 분할해 흩뜨립니다. 세부 설정은 <see cref="ExplodeOption.Default"/>에서 시작해 필요한 것만 바꿉니다
        /// </summary>
        public static void Explode(SpriteRenderer spriteRenderer, Vector3 position, ExplodeOption option)
        {
            SpriteExplosionPool.Spawn(spriteRenderer, position, option);
        }

        /// <summary>
        /// 힘이 빠지며 부서지는 연출 - 약한 힘, 위로 떠오름, 페이드 아웃
        /// </summary>
        public static void Dissolve(SpriteRenderer spriteRenderer, Vector3 position, string pieceLayerName)
        {
            SpriteExplosionPool.Spawn(spriteRenderer, position, ExplodeOption.Dissolve(pieceLayerName));
        }

        /// <summary>미리 조각을 만들어 두어 첫 폭발에서 생기는 순간 부하를 없앱니다</summary>
        public static void Prewarm(int pieceCount) => SpriteExplosionPool.Prewarm(pieceCount);

        /// <summary>화면에 남아 있는 모든 파편을 즉시 회수합니다. 스테이지 리셋/시뮬레이션 종료에 사용합니다</summary>
        public static void ClearAll() => SpriteExplosionPool.ClearAll();
    }

    /// <summary>
    /// <see cref="SpriteExploder"/>가 사용하는 파편 풀입니다
    /// </summary>
    /// <remarks>
    /// 프로젝트가 파편 생성 시점에 개입해야 하면 이 클래스를 상속한 뒤
    /// <see cref="UsePool{T}"/>로 등록한다. 그럴 필요가 없다면 그대로 두면 된다.
    /// </remarks>
    public class SpriteExplosionPool : MonoBehaviour
    {
        /// <summary>프로젝트가 지정하지 않았을 때 쓰는 파편 상한입니다</summary>
        private const int DEFAULT_MAX_PIECE_COUNT = 1024;

        /// <summary>프로젝트가 지정하지 않았을 때 쓰는 콜라이더 예산. 0이면 제한 없음(기존 동작)</summary>
        private const int DEFAULT_MAX_COLLIDING_PIECES = 0;

        private static SpriteExplosionPool _instance;
        private static Type _poolType = typeof(SpriteExplosionPool);
        private static int _maxPieceCount = DEFAULT_MAX_PIECE_COUNT;
        private static int _maxCollidingPieces = DEFAULT_MAX_COLLIDING_PIECES;

        /// <summary>
        /// 풀이 무한히 커지지 않도록 두는 상한입니다. 넘으면 가장 오래된 파편을 빼앗아 재활용합니다
        /// </summary>
        /// <remarks>
        /// 필요한 값은 "동시에 살아 있을 수 있는 파편 수"이고, 그건 프로젝트의 연출 밀도가 정한다.
        /// (한 번에 터질 수 있는 최대 개수) × (분할 수의 제곱) 이상으로 잡아야 파편이 날아가다 사라지지 않는다.
        /// 모자라면 크래시 대신 오래된 파편이 조용히 회수되므로, 증상이 성능이 아니라 연출로 나타난다.
        /// </remarks>
        public static int MaxPieceCount
        {
            get => _maxPieceCount;
            set => _maxPieceCount = Mathf.Max(1, value);
        }

        /// <summary>
        /// 콜라이더를 켠 채 동시에 물리를 돌 수 있는 파편 수의 상한입니다. 0 이하면 제한하지 않습니다
        /// </summary>
        /// <remarks>
        /// 파편 물리 비용은 개수에 정비례하지 않는다. 날아가는 동안은 싸지만 바닥에 쌓여 서로 맞닿으면
        /// 접촉을 매 스텝 다시 풀어야 해서 급격히 비싸지고, 물리가 프레임을 넘기면 엔진이 따라잡으려
        /// FixedUpdate를 더 돌리며 악순환에 빠진다.
        ///
        /// 그래서 "몇 개까지 진짜 물리를 시킬지"를 정해 두고 나머지는 콜라이더 없이 날린다.
        /// 예산을 넘겨 만들어진 파편은 지형을 통과하지만, 수백 개가 흩날리는 중에는 눈에 띄지 않는다.
        /// 안착해서 물리에서 빠진 파편은 자리를 돌려주므로 뒤에 터진 파편이 그 자리를 쓴다.
        /// </remarks>
        public static int MaxCollidingPieces
        {
            get => _maxCollidingPieces;
            set => _maxCollidingPieces = value;
        }

        /// <summary>분할된 스프라이트 캐시의 키. 같은 원본이라도 분할 수가 다르면 다른 결과다</summary>
        private readonly struct SliceKey : IEquatable<SliceKey>
        {
            private readonly SpriteId _spriteId;
            private readonly int _sliceCount;

            public SliceKey(SpriteId spriteId, int sliceCount)
            {
                _spriteId = spriteId;
                _sliceCount = sliceCount;
            }

            public bool Equals(SliceKey other) => _spriteId == other._spriteId && _sliceCount == other._sliceCount;
            public override bool Equals(object obj) => obj is SliceKey other && Equals(other);
            public override int GetHashCode() => (_spriteId.GetHashCode() * 397) ^ _sliceCount;
        }

        private sealed class Piece
        {
            public GameObject GameObject;
            public Transform Transform;
            public SpriteRenderer Renderer;
            public Rigidbody2D Body;
            public BoxCollider2D Collider;

            public Color BaseColor;
            public float SpawnTime;
            public float FadeDelay;
            public float FadeDuration;
            public float MaxLifetime;
            public bool UseFade;
            public bool UseUnscaledTime;

            /// <summary>콜라이더 예산 한 자리를 점유하고 있는지. 안착해서 물리에서 빠지면 자리를 돌려준다</summary>
            public bool HoldsColliderSlot;

            public float SettleSpeed;
            public float SettleHold;

            /// <summary>느려지기 시작한 시각. 다시 빨라지면 -1로 되돌린다</summary>
            public float SlowSince;
        }

        private readonly Dictionary<SliceKey, Sprite[]> _sliceCache = new();
        private readonly HashSet<string> _warnedLayerNames = new();
        private readonly Stack<Piece> _available = new();
        private readonly List<Piece> _active = new();
        private int _totalPieceCount;

        /// <summary>지금 콜라이더를 켠 채 물리를 돌고 있는 파편 수</summary>
        private int _collidingPieceCount;

        static SpriteExplosionPool()
        {
            // Domain Reload를 껐을 때 이전 플레이 세션의 인스턴스 참조가 남지 않게 한다
            SingletonReset.Register(typeof(SpriteExplosionPool), ResetStatics);
        }

        private static void ResetStatics()
        {
            _instance = null;
            _poolType = typeof(SpriteExplosionPool);
            _maxPieceCount = DEFAULT_MAX_PIECE_COUNT;
            _maxCollidingPieces = DEFAULT_MAX_COLLIDING_PIECES;
        }

        /// <summary>
        /// 기본 풀 대신 상속한 풀을 사용하도록 등록합니다. 첫 폭발이 일어나기 전에 호출해야 합니다
        /// </summary>
        public static void UsePool<T>() where T : SpriteExplosionPool
        {
            if (_poolType == typeof(T))
            {
                return;
            }

            _poolType = typeof(T);

            if (_instance != null)
            {
                Destroy(_instance.gameObject);
                _instance = null;
            }
        }

        public static SpriteExplosionPool Instance => GetOrCreate();

        private static SpriteExplosionPool GetOrCreate()
        {
            if (_instance != null)
            {
                return _instance;
            }

            var poolObject = new GameObject("[Pool] Sprite Explosion");
            DontDestroyOnLoad(poolObject);

            _instance = poolObject.AddComponent(_poolType) as SpriteExplosionPool;
            _instance.OnPoolCreated();

            return _instance;
        }

        /// <summary>풀 오브젝트가 만들어진 직후에 호출됩니다. 상속한 풀이 초기 설정을 붙이는 자리입니다</summary>
        protected virtual void OnPoolCreated() { }

        /// <summary>파편 하나를 배치한 직후에 호출됩니다. 상속한 풀이 추가 연출을 붙이는 자리입니다</summary>
        protected virtual void OnPieceSpawned(GameObject piece) { }

        public static void Prewarm(int pieceCount)
        {
            var pool = GetOrCreate();

            for (int i = pool._totalPieceCount; i < pieceCount && i < _maxPieceCount; i++)
            {
                pool._available.Push(pool.CreatePiece());
            }
        }

        public static void ClearAll()
        {
            if (_instance == null)
            {
                return;
            }

            _instance.ClearAllInternal();
        }

        private void ClearAllInternal()
        {
            for (int i = _active.Count - 1; i >= 0; i--)
            {
                Return(_active[i]);
            }

            _active.Clear();
        }

        public static void Spawn(SpriteRenderer spriteRenderer, Vector3 position, ExplodeOption option)
        {
            if (spriteRenderer == null || spriteRenderer.sprite == null)
            {
                return;
            }

            if (option.SliceCount < 1)
            {
                return;
            }

            GetOrCreate().SpawnInternal(spriteRenderer, position, option);
        }

        private void SpawnInternal(SpriteRenderer source, Vector3 position, ExplodeOption option)
        {
            var sprite = source.sprite;
            int sliceCount = ClampSliceCount(sprite, option.SliceCount);
            var slices = GetSlices(sprite, sliceCount);

            int layer = ResolvePieceLayer(option.PieceLayerName);

            int sortingOrder = option.SortingOrderOverride != ExplodeOption.NO_SORTING_ORDER_OVERRIDE
                ? option.SortingOrderOverride
                : source.sortingOrder + option.SortingOrderOffset;

            float pieceWidth = sprite.bounds.size.x / sliceCount;
            float pieceHeight = sprite.bounds.size.y / sliceCount;

            // 조각의 피벗은 중심이므로 원본의 좌하단에서 반 칸 안쪽이 첫 조각의 중심이 된다
            var origin = position - sprite.bounds.extents + new Vector3(pieceWidth * 0.5f, pieceHeight * 0.5f, 0f);

            float now = option.Fade.UseUnscaledTime ? Time.unscaledTime : Time.time;
            var colliderSize = new Vector2(pieceWidth, pieceHeight);

            int sliceIndex = 0;
            for (int y = 0; y < sliceCount; y++)
            {
                for (int x = 0; x < sliceCount; x++)
                {
                    var piece = Rent();

                    piece.Transform.position = origin + new Vector3(x * pieceWidth, y * pieceHeight, 0f);
                    piece.Transform.rotation = Quaternion.identity;
                    piece.GameObject.layer = layer;

                    piece.Renderer.sprite = slices[sliceIndex++];
                    piece.Renderer.sortingLayerID = source.sortingLayerID;
                    piece.Renderer.sortingOrder = sortingOrder;

                    if (option.PieceMaterial != null)
                    {
                        piece.Renderer.sharedMaterial = option.PieceMaterial;
                    }

                    piece.BaseColor = source.color;
                    piece.Renderer.color = source.color;

                    // 콜라이더는 예산이 남아 있을 때만 준다. 넘친 파편은 지형을 통과해 날아간다
                    bool wantsCollider = option.HasCollider
                        && (_maxCollidingPieces <= 0 || _collidingPieceCount < _maxCollidingPieces);

                    piece.Collider.enabled = wantsCollider;
                    if (wantsCollider)
                    {
                        piece.Collider.size = colliderSize;
                        piece.Collider.offset = Vector2.zero;
                        piece.HoldsColliderSlot = true;
                        _collidingPieceCount++;
                    }

                    piece.SettleSpeed = option.SettleSpeed;
                    piece.SettleHold = option.SettleHold;
                    piece.SlowSince = -1f;

                    piece.SpawnTime = now;
                    piece.FadeDelay = option.Fade.FadeDelay;
                    piece.FadeDuration = option.Fade.FadeDuration;
                    piece.UseFade = option.Fade.UseFade;
                    piece.UseUnscaledTime = option.Fade.UseUnscaledTime;
                    piece.MaxLifetime = option.MaxLifetime;

                    piece.GameObject.SetActive(true);

                    // 물리 상태는 활성화 이후에 넣어야 이전 파편의 속도가 남지 않는다
                    piece.Body.simulated = true;
                    piece.Body.gravityScale = option.GravityScale;
                    piece.Body.linearVelocity = Vector2.zero;
                    piece.Body.angularVelocity = 0f;

                    var direction = new Vector2(
                        UnityEngine.Random.Range(option.ForceRangeX.x, option.ForceRangeX.y),
                        UnityEngine.Random.Range(option.ForceRangeY.x, option.ForceRangeY.y)
                    ).normalized;

                    piece.Body.AddForce(direction * option.Force);
                    piece.Body.AddTorque(UnityEngine.Random.Range(-option.TorqueRange, option.TorqueRange));

                    _active.Add(piece);
                    OnPieceSpawned(piece.GameObject);
                }
            }
        }

        private void Update()
        {
            float scaledNow = Time.time;
            float unscaledNow = Time.unscaledTime;

            for (int i = _active.Count - 1; i >= 0; i--)
            {
                var piece = _active[i];
                float now = piece.UseUnscaledTime ? unscaledNow : scaledNow;
                float elapsed = now - piece.SpawnTime;

                TrySettle(piece, now);

                if (!piece.UseFade)
                {
                    // 페이드가 없으면 기한이 다 될 때까지 그대로 두고, 기한이 없으면 ClearAll을 기다린다
                    if (piece.MaxLifetime > 0f && elapsed >= piece.MaxLifetime)
                    {
                        Return(piece);
                        _active.RemoveAt(i);
                    }

                    continue;
                }

                if (elapsed <= piece.FadeDelay)
                {
                    continue;
                }

                float duration = piece.FadeDuration > 0f ? piece.FadeDuration : 0.001f;
                float progress = (elapsed - piece.FadeDelay) / duration;

                if (progress >= 1f)
                {
                    Return(piece);
                    _active.RemoveAt(i);
                    continue;
                }

                var color = piece.BaseColor;
                color.a = piece.BaseColor.a * (1f - progress);
                piece.Renderer.color = color;
            }
        }

        /// <summary>
        /// 다 굴러서 멈춘 파편을 물리 시뮬레이션에서 빼고 콜라이더 예산 자리를 돌려줍니다
        /// </summary>
        /// <remarks>
        /// 물리를 끄면 그 자리에 그대로 서 있으므로 화면에 보이는 모습은 달라지지 않는다.
        /// 뒤이어 터진 파편이 이 자리를 넘겨받아 콜라이더를 얻는다.
        /// </remarks>
        private void TrySettle(Piece piece, float now)
        {
            if (!piece.HoldsColliderSlot || piece.SettleSpeed <= 0f)
            {
                return;
            }

            // 회전만 남아 제자리에서 도는 경우도 멈춘 것으로 본다
            bool slow = piece.Body.linearVelocity.sqrMagnitude <= piece.SettleSpeed * piece.SettleSpeed;

            if (!slow)
            {
                piece.SlowSince = -1f;
                return;
            }

            if (piece.SlowSince < 0f)
            {
                piece.SlowSince = now;
                return;
            }

            if (now - piece.SlowSince < piece.SettleHold)
            {
                return;
            }

            piece.Body.linearVelocity = Vector2.zero;
            piece.Body.angularVelocity = 0f;
            piece.Body.simulated = false;

            piece.HoldsColliderSlot = false;
            _collidingPieceCount--;
        }

        /// <summary>
        /// 텍스처가 감당할 수 있는 분할 수로 제한합니다
        /// </summary>
        /// <remarks>
        /// 조각 하나가 1픽셀 미만이 되면 Sprite.Create가 만들 수 없는 rect가 나온다.
        /// 32×32 스프라이트를 40등분해 달라는 요청은 32등분으로 받아 준다.
        /// </remarks>
        private static int ClampSliceCount(Sprite sprite, int requested)
        {
            var textureRect = sprite.textureRect;
            int limit = Mathf.Max(1, Mathf.FloorToInt(Mathf.Min(textureRect.width, textureRect.height)));

            return Mathf.Clamp(requested, 1, limit);
        }

        /// <summary>
        /// 파편 레이어 이름을 인덱스로 바꾸고, 없는 이름이면 이름당 한 번씩 경고합니다
        /// </summary>
        /// <remarks>
        /// 이름이 틀리면 파편은 Default 레이어로 떨어져 서로 충돌하고 플레이어를 밀어낸다.
        /// 증상은 크지만 원인이 드러나지 않는 부류라, 조용히 넘어가지 않고 반드시 말해 준다.
        /// 폭발마다 찍으면 로그가 잠기므로 이름당 한 번만 남긴다.
        /// </remarks>
        private int ResolvePieceLayer(string pieceLayerName)
        {
            int layer = string.IsNullOrEmpty(pieceLayerName) ? -1 : LayerMask.NameToLayer(pieceLayerName);

            if (layer >= 0)
            {
                return layer;
            }

            if (_warnedLayerNames.Add(pieceLayerName ?? string.Empty))
            {
                Debug.LogWarning($"[{nameof(SpriteExplosionPool)}] '{pieceLayerName}' 레이어를 찾을 수 없어 파편을 Default 레이어에 올립니다 - "
                    + "파편끼리 충돌하고 플레이어를 밀어냅니다. 프로젝트에 레이어를 추가하거나 ExplodeOption.PieceLayerName을 고치세요");
            }

            return 0;
        }

        private Piece Rent()
        {
            if (_available.Count > 0)
            {
                return _available.Pop();
            }

            if (_totalPieceCount < _maxPieceCount)
            {
                return CreatePiece();
            }

            // 상한에 걸렸으면 가장 오래된 파편을 빼앗아 재사용한다
            var oldest = _active[0];
            _active.RemoveAt(0);
            Return(oldest);

            return _available.Pop();
        }

        private Piece CreatePiece()
        {
            var pieceObject = new GameObject("Piece");
            pieceObject.transform.SetParent(transform);
            pieceObject.SetActive(false);

            var piece = new Piece
            {
                GameObject = pieceObject,
                Transform = pieceObject.transform,
                Renderer = pieceObject.AddComponent<SpriteRenderer>(),
                Body = pieceObject.AddComponent<Rigidbody2D>(),
                Collider = pieceObject.AddComponent<BoxCollider2D>()
            };

            piece.Collider.enabled = false;
            piece.Body.simulated = false;

            _totalPieceCount++;

            return piece;
        }

        private void Return(Piece piece)
        {
            if (piece.HoldsColliderSlot)
            {
                piece.HoldsColliderSlot = false;
                _collidingPieceCount--;
            }

            piece.Body.linearVelocity = Vector2.zero;
            piece.Body.angularVelocity = 0f;
            piece.Body.simulated = false;
            piece.Collider.enabled = false;
            piece.GameObject.SetActive(false);

            _available.Push(piece);
        }

        private Sprite[] GetSlices(Sprite source, int sliceCount)
        {
#if UNITY_6000_5_OR_NEWER
            var key = new SliceKey(source.GetEntityId(), sliceCount);
#else
            var key = new SliceKey(source.GetInstanceID(), sliceCount);
#endif

            if (_sliceCache.TryGetValue(key, out var cached))
            {
                return cached;
            }

            var slices = new Sprite[sliceCount * sliceCount];

            var texture = source.texture;
            // textureRect를 써야 아틀라스에 패킹된 스프라이트도 자기 영역만 잘라낸다
            var textureRect = source.textureRect;
            var pivot = new Vector2(0.5f, 0.5f);

            // 텍스처 밖으로는 단 한 픽셀도 나가면 안 되므로 상한을 미리 잡아 둔다
            float limitX = Mathf.Min(textureRect.xMax, texture.width);
            float limitY = Mathf.Min(textureRect.yMax, texture.height);

            int index = 0;
            for (int y = 0; y < sliceCount; y++)
            {
                // 조각 폭을 누적해 더하면 부동소수 오차가 쌓여 마지막 조각이 텍스처 밖으로 삐져나가고
                // Sprite.Create가 그 rect를 거부한다(256px을 7등분: 6*36.571428 + 36.571428 > 256).
                // 그래서 매 경계를 전체 크기에서 다시 계산하고, 끝은 텍스처 경계로 자른다.
                float minY = textureRect.y + textureRect.height * y / sliceCount;
                float maxY = Mathf.Min(textureRect.y + textureRect.height * (y + 1) / sliceCount, limitY);

                for (int x = 0; x < sliceCount; x++)
                {
                    float minX = textureRect.x + textureRect.width * x / sliceCount;
                    float maxX = Mathf.Min(textureRect.x + textureRect.width * (x + 1) / sliceCount, limitX);

                    slices[index++] = Sprite.Create(
                        texture, Rect.MinMaxRect(minX, minY, maxX, maxY), pivot, source.pixelsPerUnit);
                }
            }

            _sliceCache[key] = slices;

            return slices;
        }

        private void OnDestroy()
        {
            if (_instance == this)
            {
                _instance = null;
            }

            // Sprite.Create로 만든 스프라이트는 GC 대상이 아니라서 직접 지우지 않으면 남는다
            foreach (var slices in _sliceCache.Values)
            {
                for (int i = 0; i < slices.Length; i++)
                {
                    if (slices[i] != null)
                    {
                        Destroy(slices[i]);
                    }
                }
            }

            _sliceCache.Clear();
        }
    }
}
