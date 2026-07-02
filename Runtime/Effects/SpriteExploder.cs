using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace JoonyleGameDevKit
{
    [System.Serializable]
    public struct FadeOption
    {
        public bool UseFade;
        public float FadeDelay;
        public float FadeDuration;

        public static FadeOption None => new FadeOption
        {
            UseFade = false,
            FadeDelay = 0f,
            FadeDuration = 0f
        };

        public static FadeOption Default => new FadeOption
        {
            UseFade = true,
            FadeDelay = 5f,
            FadeDuration = 0.5f
        };
    }

    /// <summary>
    /// 스프라이트를 N×N 조각으로 분할하여 폭발시키는 유틸리티
    /// </summary>
    public static class SpriteExploder
    {
        public static void Explode(SpriteRenderer spriteRenderer, Vector3 position,
            int sliceCount = 4, float force = 50f, float gravityScale = 1f,
            bool hasCollider = true, FadeOption fadeOption = default, string pieceLayerName = "SpritePiece")
        {
            var root = new GameObject("ExplodePieces");
            var pieces = CreatePieces(spriteRenderer, root.transform, position, sliceCount,
                hasCollider: hasCollider, sortingOrderOffset: 0, gravityScale: gravityScale, force: force,
                forceRangeX: new Vector2(-1f, 1f), forceRangeY: new Vector2(0.5f, 1f), torqueRange: 200f,
                pieceLayerName: pieceLayerName);

            if (fadeOption.UseFade)
            {
                var runner = root.AddComponent<BatchFadeRunner>();
                runner.Initialize(pieces, fadeOption.FadeDelay, fadeOption.FadeDuration);
            }
        }

        /// <summary>
        /// 힘이 빠지며 부서지는 연출 — 약한 힘, 위로 떠오름, 페이드 아웃
        /// </summary>
        public static void Dissolve(SpriteRenderer spriteRenderer, Vector3 position,
            int sliceCount = 4, float force = 50f, float gravityScale = -0.1f,
            FadeOption? fadeOption = null, string pieceLayerName = "SpritePiece")
        {
            var option = fadeOption ?? new FadeOption { UseFade = true, FadeDelay = 0.3f, FadeDuration = 0.6f };

            var root = new GameObject("DissolvePieces");
            var pieces = CreatePieces(spriteRenderer, root.transform, position, sliceCount,
                hasCollider: false, sortingOrderOffset: 1, gravityScale: gravityScale, force: force,
                forceRangeX: new Vector2(-0.5f, 0.5f), forceRangeY: new Vector2(0.3f, 1f), torqueRange: 50f,
                pieceLayerName: pieceLayerName);

            if (option.UseFade)
            {
                var runner = root.AddComponent<BatchFadeRunner>();
                runner.Initialize(pieces, option.FadeDelay, option.FadeDuration);
            }
        }

        /// <summary>
        /// 스프라이트를 N×N 조각으로 슬라이스하여 각각에 SpriteRenderer/Rigidbody2D를 붙이고 힘을 가한다
        /// </summary>
        private static List<SpriteRenderer> CreatePieces(SpriteRenderer spriteRenderer, Transform parent, Vector3 position,
            int sliceCount, bool hasCollider, int sortingOrderOffset, float gravityScale, float force,
            Vector2 forceRangeX, Vector2 forceRangeY, float torqueRange, string pieceLayerName)
        {
            var sprite = spriteRenderer.sprite;
            var texture = sprite.texture;
            var textureRect = sprite.textureRect;

            float spriteWidth = sprite.bounds.size.x;
            float spriteHeight = sprite.bounds.size.y;
            float pieceWidth = spriteWidth / sliceCount;
            float pieceHeight = spriteHeight / sliceCount;

            float texturePieceWidth = textureRect.width / sliceCount;
            float texturePieceHeight = textureRect.height / sliceCount;

            var pieces = new List<SpriteRenderer>();
            var spritePieceLayer = LayerMask.NameToLayer(pieceLayerName);
            var layer = spritePieceLayer >= 0 ? spritePieceLayer : 0;

            for (int x = 0; x < sliceCount; x++)
            {
                for (int y = 0; y < sliceCount; y++)
                {
                    var pieceSprite = Sprite.Create(
                        texture,
                        new Rect(
                            textureRect.x + x * texturePieceWidth,
                            textureRect.y + y * texturePieceHeight,
                            texturePieceWidth,
                            texturePieceHeight
                        ),
                        new Vector2(0.5f, 0.5f),
                        sprite.pixelsPerUnit
                    );

                    var piece = new GameObject($"Piece_{x}_{y}");
                    piece.layer = layer;
                    piece.transform.SetParent(parent);
                    piece.transform.position = new Vector2(
                        position.x + (x - sliceCount / 2f) * pieceWidth,
                        position.y + (y - sliceCount / 2f) * pieceHeight
                    );

                    var pieceSr = piece.AddComponent<SpriteRenderer>();
                    pieceSr.sprite = pieceSprite;
                    pieceSr.sortingLayerName = spriteRenderer.sortingLayerName;
                    pieceSr.sortingOrder = spriteRenderer.sortingOrder + sortingOrderOffset;

                    if (hasCollider)
                    {
                        piece.AddComponent<BoxCollider2D>();
                    }

                    var rigidbody = piece.AddComponent<Rigidbody2D>();
                    rigidbody.gravityScale = gravityScale;
                    Vector2 dir = new Vector2(
                        Random.Range(forceRangeX.x, forceRangeX.y),
                        Random.Range(forceRangeY.x, forceRangeY.y)
                    ).normalized;
                    rigidbody.AddForce(dir * force);
                    rigidbody.AddTorque(Random.Range(-torqueRange, torqueRange));

                    pieces.Add(pieceSr);
                }
            }

            return pieces;
        }

        /// <summary>
        /// 부모 오브젝트에 부착되어 자식 조각들을 일괄 페이드 아웃 후 파괴
        /// </summary>
        private class BatchFadeRunner : MonoBehaviour
        {
            private List<SpriteRenderer> _pieces;
            private float _fadeDelay;
            private float _fadeDuration;

            public void Initialize(List<SpriteRenderer> pieces, float fadeDelay, float fadeDuration)
            {
                _pieces = pieces;
                _fadeDelay = fadeDelay;
                _fadeDuration = fadeDuration;
                StartCoroutine(FadeAndDestroyAll());
            }

            private IEnumerator FadeAndDestroyAll()
            {
                yield return new WaitForSecondsRealtime(_fadeDelay);

                float elapsedTime = 0f;
                while (elapsedTime < _fadeDuration)
                {
                    elapsedTime += Time.unscaledDeltaTime;
                    float alpha = Mathf.Lerp(1f, 0f, elapsedTime / _fadeDuration);

                    foreach (var sr in _pieces)
                    {
                        if (sr == null) continue;
                        var color = sr.color;
                        color.a = alpha;
                        sr.color = color;
                    }

                    yield return null;
                }

                // 부모 파괴 → 자식 조각 전부 정리
                Destroy(gameObject);
            }
        }
    }
}
