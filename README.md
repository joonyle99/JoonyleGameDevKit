# JoonyleGameDevKit

Unity 게임 개발을 위한 유틸리티 패키지입니다.

## Installation

Unity Package Manager에서 Git URL로 설치할 수 있습니다.

1. Window > Package Manager 열기
2. '+' 버튼 클릭 > "Add package from git URL..." 선택
3. 아래 URL 입력:

```
https://github.com/joonyle99/JoonyleGameDevKit.git
```

특정 버전을 설치하려면 태그를 지정합니다:

```
https://github.com/joonyle99/JoonyleGameDevKit.git#v1.0.0
```

## Requirements

- Unity 6000.0 이상

## Features

### Singleton

MonoBehaviour 기반 싱글톤 패턴 구현체입니다.

| 클래스 | 설명 |
|--------|------|
| `StaticInstance<T>` | 기본 정적 인스턴스. 새 인스턴스가 기존 인스턴스를 덮어씁니다. |
| `Singleton<T>` | 기본 싱글톤. 새 인스턴스 생성 시 파괴됩니다. |
| `PersistentSingleton<T>` | 씬 전환에도 유지되는 싱글톤. |

```csharp
public class GameManager : Singleton<GameManager>
{
    // GameManager.Instance로 접근
}

public class AudioManager : PersistentSingleton<AudioManager>
{
    // 씬 전환에도 유지됨
}
```

### ObjectPooling

오브젝트 풀링을 통한 성능 최적화를 지원합니다.

```csharp
public class BulletSpawner : MonoBehaviour
{
    [SerializeField] private ObjectPooling bulletPool;

    void Fire()
    {
        var bullet = bulletPool.GetObject<Bullet>();
        // 사용 후 반환
        bulletPool.ReturnObject(bullet);
    }
}
```

**주요 기능:**
- 자동 풀 확장 (50%씩 증가)
- 제네릭 컴포넌트 반환
- 디버그 로깅 지원

### Utility

#### ExtensionMethod

Vector 변환 및 유틸리티 확장 메서드입니다.

```csharp
// Vector 변환
Vector3 v3 = someVector2.ToVector3();
Vector2 v2 = someVector3.ToVector2();
Vector3Int v3i = someVector3.ToVector3Int();

// Rect에서 랜덤 좌표
Vector2 randomPoint = someRect.GetRandomPoint();

// LayerMask 변환
int layerNumber = layerMaskValue.GetLayerNumber();

// 문자열에서 숫자 추출
int num = "Item_123".ExtractNumber(); // 123
```

#### Mathematics

Easing 함수 및 베지어 곡선 계산을 제공합니다.

```csharp
// Easing 함수
float eased = Mathematics.EaseInQuad(t);
float eased = Mathematics.EaseOutBack(t);

// 2차 베지어 곡선
Vector3 point = Mathematics.CalcBezier2Point(p0, p1, p2, t);
```

**지원하는 Easing:**
- EaseIn: Quad, Cubic, Quart, Expo, Circ, Back
- EaseOut: Quad, Cubic, Quart, Expo, Circ, Back

#### Painter

디버그 및 기즈모 그리기 유틸리티입니다.

```csharp
// Debug.DrawLine 기반
Painter.DebugDrawX(center, Color.red);
Painter.DebugDrawPlus(center, Color.blue);

// Gizmos 기반
Painter.GizmosDrawArrow(start, direction);
Painter.GizmosDrawVerticalAxis(center);
```

#### Types

커스텀 타입 정의입니다.

| 타입 | 설명 |
|------|------|
| `RangeFloat` | float 범위 및 랜덤 값 생성 |
| `RangeInt` | int 범위 및 랜덤 값 생성 |
| `Line2D` | 2D 선분 및 보간 |
| `Line3D` | 3D 선분 및 보간 |

```csharp
[SerializeField] private RangeFloat speedRange;

void Start()
{
    float randomSpeed = speedRange.Random();
}
```

## License

MIT License - 자세한 내용은 [LICENSE](LICENSE) 파일을 참조하세요.

## Author

- [joonyle99](https://github.com/joonyle99)
