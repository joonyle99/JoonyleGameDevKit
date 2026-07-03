# Changelog

All notable changes to this package will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

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
