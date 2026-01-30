# Changelog

All notable changes to this package will be documented in this file.

The format is based on [Keep a Changelog](http://keepachangelog.com/en/1.0.0/)
and this project adheres to [Semantic Versioning](http://semver.org/spec/v2.0.0.html).

## [1.2.0] - 2026-01-30

### Added

- `Transition<T>`: 상태 전이 조건을 관리하는 클래스
- `StateMachine<T>.AddTransition<TFrom, TTo>(condition)`: 전이 조건 등록 메서드
- FSM 한계점 및 권장 사용 사례에 대한 문서화 주석

### Changed

- State 예제 폴더 이름 변경: `Behaviour` → `Example`

### Removed

- `IState<T>` 인터페이스 제거 (`StateBase<T>`만 사용)

## [1.1.0] - 2026-01-30

### Added

- **State** 모듈
  - `IState<T>`: 상태 인터페이스
  - `StateBase<T>`: 상태 기본 추상 클래스
  - `StateMachine<T>`: 유한 상태 머신 (FSM)

## [1.0.0] - 2026-01-29

### Added

- **Singleton** 모듈
  - `StaticInstance<T>`: 정적 인스턴스 기본 클래스
  - `Singleton<T>`: 기본 싱글톤 패턴
  - `PersistentSingleton<T>`: 씬 전환에도 유지되는 싱글톤

- **ObjectPooling** 모듈
  - `ObjectPooling`: 오브젝트 풀 관리 컴포넌트
  - `ItemGenerator`: 아이템 생성기
  - `ItemObject`: 풀링 가능한 아이템 오브젝트
  - `IGeneratable`: 생성 가능한 객체 인터페이스

- **Utility** 모듈
  - `ExtensionMethod`: Vector, Rect, LayerMask, String 확장 메서드
  - `Mathematics`: Easing 함수 (EaseIn/Out) 및 베지어 곡선 계산
  - `Painter`: Debug.DrawLine 및 Gizmos 그리기 유틸리티
  - `Types`: RangeFloat, RangeInt, Line2D, Line3D 커스텀 타입
