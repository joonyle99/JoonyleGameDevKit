# Changelog

All notable changes to this package will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [2.0.3] - 2026-09-22

### Fixed
- Unity 6000.5에서 `FindObjectsSortMode` 오버로드가 Obsolete가 되어 `SoundManagerBase.SetGamePaused`가 경고를 뱉던 문제. 6000.5 이상에서는 정렬 인자 없는 오버로드를 쓰고, 구버전 경로는 그대로 유지한다 (동작 동일 — 원래도 `SortMode.None`이었다)

## [2.0.2] - 2026-09-22

### Changed
- `SingletonReset.ResetAll`이 되돌릴 타입이 하나도 없으면 에디터 진단 로그를 남기지 않는다 (도메인이 새로 생성된 직후의 첫 진입에서 알릴 내용 없는 로그가 찍히던 것을 정리)

## [2.0.1] - 2026-09-22

### Changed
- `RuntimeInitializer`가 `Resources/Prefabs/Bootstrapper` 프리팹을 찾지 못해도 에러 로그 없이 조용히 넘어간다. Bootstrapper는 이제 선택 기능이며, 쓰려는 프로젝트만 프리팹을 두면 된다 (Bootstrapper 샘플 임포트)

## [2.0.0] - 2026-09-20

### Changed
- **BREAKING** `SpriteExploder.Explode`/`Dissolve`가 개별 인자 대신 `ExplodeOption`을 받도록 변경. 파편 레이어 기본값을 없애고 `ExplodeOption.Default(pieceLayerName)`처럼 레이어 이름을 필수 인자로 요구한다 (프로젝트마다 레이어 이름이 달라 기본값을 두면 레이어를 못 찾는 실수가 조용히 넘어갔다)
- `SpriteExploder`를 풀링 기반으로 재작성 — 조각 GameObject와 분할 Sprite를 `SpriteExplosionPool`에서 재사용해 폭발마다 생성/파괴하지 않는다
- `SingletonReset`의 에디터 진단 로그 색상을 cyan으로 변경

### Added
- `ExplodeOption.PieceMaterial` — 파편 렌더 배칭에 쓸 머티리얼을 프로젝트가 지정
- `ExplodeOption.MaxLifetime`, `FadeOption.UseUnscaledTime` — 페이드가 없는 파편도 반드시 회수되도록 보장
- `SpriteExplosionPool.MaxPieceCount`, `MaxCollidingPieces` — 파편이 지형에 쌓일 때 접촉 유지 비용이 급격히 커지는 것을 막는 상한. 다 굴러 멈춘 파편은 물리에서 빼 콜라이더 예산 자리를 반납한다
- `SpriteExplosionPool.UsePool<T>()`와 상속 훅 `OnPoolCreated`/`OnPieceSpawned` — 프로젝트가 풀 생성 시점에 개입 가능
- `SpriteExploder.Dissolve()` 복원 (풀을 통해 동작)

### Fixed
- Unity 6000.5부터 `Object.GetInstanceID()`가 `GetEntityId()`로 대체되며 반환 타입이 `EntityId`가 되어, 분할 스프라이트 캐시 키가 컴파일되지 않던 문제 (구버전 동작은 그대로)
- 파편 조각이 원본 bounds 밖으로 반 칸 밀리던 위치 계산 버그
- 텍스처 크기가 분할 수로 나누어떨어지지 않으면 `Sprite.Create`의 부동소수 오차로 마지막 조각이 텍스처를 벗어나 예외가 나던 문제

## [1.9.1] - 2026-08-10

### Changed
- `StaticInstance<T>`가 `SingletonReset`에 등록하는 키를 `StaticInstance<T>` 대신 `T`로 변경해 실제 싱글톤 타입 이름이 드러나도록 정리
- `SingletonReset.ResetAll`에 에디터 전용 진단 로그 추가 (이번 플레이 모드 진입에서 되돌린 타입 목록 출력)

## [1.9.0] - 2026-08-10

### Added
- `Runtime/Singleton` — `SingletonReset`(플레이 모드 진입 시 실행할 정적 상태 리셋 동작을 등록하는 등록소. `RuntimeInitializeOnLoadMethod`를 쓸 수 없는 제네릭 클래스를 대신해 호출)

### Fixed
- Domain Reload를 끈 상태에서 플레이 모드에 재진입하면 `StaticInstance<T>`의 `_instance`/`_isQuitting`이 이전 세션 값을 유지해 `Instance`가 계속 `null`을 반환하던 문제

## [1.8.1] - 2026-07-30

### Fixed
- `IrisMaskTransition`의 `_openScale`/`_duration`/`_easeIn`/`_easeOut`/`_callbackDelayTime` 기본값 튜닝

## [1.8.0] - 2026-07-30

### Added
- `Runtime/Effect` — `IrisMaskTransition`(focus 위치를 추적하는 원형 SpriteMask를 확대/축소해 화면을 열고 닫는 이리스 전환 연출)

### Changed
- `Samples~/UIController`의 `InGameUIController`/`OutGameUIController`에서 `IGameStateListener<T>` 구현 제거

## [1.7.0] - 2026-07-30

### Added
- `Runtime/Camera` — `CameraControllerBase`(카메라 크기 계산 등 공통 기능 제공. 프로젝트별 이동 로직은 상속받은 `CameraController`에서 구현)
- `Runtime/Parallax` — `ParallaxBackground`/`ParallaxLayer`(카메라 이동에 따라 레이어별 이동 비율을 다르게 적용하는 패럴랙스 배경)
- `Runtime/UI/Motion` — `PanelToggler`(UI 패널 토글 모션)
- `Samples~` — Camera Controller / Game Manager / UI Controller 사용 예제 샘플 3종 등록

## [1.6.2] - 2026-07-30

### Changed
- `Runtime/State`를 `Runtime/StateMachine`(`StateBase`/`StateMachine`/`Transition`, FSM 패턴)과 `Runtime/GameState`(`GameStateController`/`IGameStateListener`, enum 기반 상태 컨트롤러)로 분리

## [1.6.1] - 2026-07-04

### Fixed
- asmdef에 `DOTweenPro.Scripts` 어셈블리 참조 추가 (DOTween Pro의 Create ASMDEF로 생성되는 스크립트 어셈블리 대응) 및 참조 순서 정리

## [1.6.0] - 2026-07-03

### Added
- `Samples~/Bootstrapper` — `Bootstrapper` 프리팹(Resources/Prefabs 경로 포함)을 미리 구성해 임포트만으로 `RuntimeInitializer`가 자동으로 로드하도록 등록

## [1.5.0] - 2026-07-03

### Added
- `Runtime/Input` — `IPointerInput`/`PointerInput`(Input System 기반 마우스/터치 통합 포인터 입력. 탭/패스트탭/드래그 판정, UI 위에서 시작한 눌림 차단)
- `Runtime/State` — `GameStateController<T>`/`IGameStateListener<T>`(enum 기반의 가벼운 씬 흐름 상태 컨트롤러)
- `Runtime/Manager/Effect` — `EffectManagerBase<TVfx, TSelf>`(프로젝트별 VFX enum을 주입받는 ObjectPool 기반 이펙트 매니저 베이스)
- `Runtime/Manager/UserData` — `UserDataManagerBase<TData, TSelf>`(유저 데이터 저장/로드 매니저 베이스)
- `Runtime/Effect` — `EffectBase`/`EffectAnimator`/`EffectParticle`(풀링 가능한 이펙트 컴포넌트 베이스)
- `Samples~` — Effect / Sound / UserData 매니저 사용 예제 샘플 3종 등록

### Changed
- `Runtime/Effects` 폴더를 `Runtime/Effect`로 이름 변경 (DamagePopup/DamagePopupPool/SpriteExploder 이동)
- asmdef에 `Unity.InputSystem` 참조 추가, UI Feedback이 사용하던 `UnityEngine.UI` 참조 누락 보완
- `package.json`에 `com.unity.inputsystem`/`com.unity.ugui` 의존성 명시

## [1.4.0] - 2026-07-03

### Added
- `Runtime/Manager/Sound` — `SoundManagerBase<TBgm, TSfx, TSelf>`(프로젝트별 BGM/SFX enum을 주입받는 사운드 매니저 베이스, DOTween 기반 BGM 페이드, on/off 설정 저장, 일시정지 지원)
- `Runtime/UI` — `UIPanel`(CanvasGroup 기반 표시/상호작용 제어), `UI/Feedback`(색상/스케일/오버레이/사운드/커스텀 함수 Selectable 피드백), `UI/Motion`(DOTween Sequence 기반 등장/유휴 모션)

### Changed
- `Joonyle.GameDevKit.Effects` 전용 asmdef를 제거하고 DOTween/TextMeshPro 참조를 메인 `Joonyle.GameDevKit` asmdef로 통합 (Sound/UI 쪽에서도 DOTween 의존이 필요해짐에 따라 어셈블리 분리 실익 감소)

## [1.3.0] - 2026-07-02

### Added
- `Runtime/Effects` — `DamagePopup`/`DamagePopupPool`(DOTween+TextMeshPro 기반 데미지 팝업 풀링), `SpriteExploder`(스프라이트 폭발/디졸브 이펙트). DOTween Pro/TextMeshPro 의존성은 `Joonyle.GameDevKit.Effects` 전용 asmdef로 분리해 메인 Runtime 어셈블리는 계속 의존성 없이 컴파일됨
- `Runtime/Utility` — `AnimationEventBridge`/`IAnimationEventHandler`(애니메이션 이벤트 브릿지), `NumberFormatter`/`TimeFormatter`(숫자·시간 포맷), `BezierCurve`(임의 차수 de Casteljau 베지어)

### Changed
- `Runtime/Utility`를 `Animation`/`Extensions`/`Formatting`/`Math`/`Debug` 기능별 서브폴더로 재구성
- 새로 추가된 모든 타입을 `JoonyleGameDevKit` 네임스페이스로 통일

### Removed
- 사용되지 않던 `SortingLayerAttribute` 삭제 (연결된 PropertyDrawer 없음)

## [1.2.1] - 2026-07-02

### Fixed
- `package.json`의 `version`이 `v1.2.0` 이후 `1.0.0`으로 되돌아가 있던 버전 역행 문제 수정

### Changed
- Example 상태머신 예제를 `Runtime/State/Example`에서 `Samples~/StateMachine` 샘플로 이동
- `package.json`의 `samples` 항목을 StateMachine 샘플 기준으로 갱신

### Added
- `LICENSE.md`, `CHANGELOG.md` 추가 (Unity Package Manager Details 탭에 표시)
