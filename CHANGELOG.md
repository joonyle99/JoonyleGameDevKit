# Changelog

All notable changes to this package will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

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
