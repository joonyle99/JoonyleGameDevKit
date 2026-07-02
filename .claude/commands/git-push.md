---
description: 변경사항을 작업 단위로 나눠 커밋하고, 버전을 갱신해 태그를 붙여 푸시
allowed-tools: Bash(git status:*), Bash(git diff:*), Bash(git add:*), Bash(git commit:*), Bash(git push:*), Bash(git branch:*), Bash(git fetch:*), Bash(git pull:*), Bash(git log:*), Bash(git tag:*), Bash(git describe:*)
---

## 현재 상태
- 변경 파일: !`git status --short`
- 변경 요약: !`git diff --stat`
- 현재 브랜치: !`git branch --show-current`
- 현재 package.json 버전: !`grep '"version"' package.json`
- 최신 태그: !`git describe --tags --abbrev=0 2>/dev/null || echo "(태그 없음)"`

## 작업
이 저장소는 Unity 프로젝트가 아니라 **git UPM 패키지**다. 커밋 후 push할 때마다 `package.json`의 `version`을 갱신하고, 그 버전에 맞는 git 태그를 만들어 함께 배포한다.

1. 변경사항이 없으면 그 사실만 알리고 종료한다.
2. 변경사항을 논리적 단위로 묶어 커밋 분리
   - `Runtime/`, `Editor/`, `Samples~/` 코드 변경은 영역별로 분리
   - `package.json`, `README.md` 등 메타/문서 변경은 별도 커밋
   - **Unity `.meta` 파일은 항상 해당 에셋과 같은 커밋에 포함**한다
   - `.asmdef` 변경은 관련 코드 변경과 함께 커밋
3. 커밋 메시지는 한 줄 요약(+필요 시 본문). 예: `feat: EventBus에 우선순위 옵션 추가`
4. 각 단위별로 파일을 명시해 `git add <파일>` → `git commit` 반복 (`git add .` 금지)
5. **버전 갱신 여부 판단**
   - 이번 변경이 배포할 가치가 있는 코드/기능 변경인지 확인한다 (문서만 고친 경우 등은 생략 가능 — 애매하면 사용자에게 묻는다)
   - 배포 대상이면 SemVer 규칙에 따라 다음 버전을 정한다:
     - `MAJOR`: 하위 호환 깨지는 API 변경
     - `MINOR`: 하위 호환되는 기능 추가
     - `PATCH`: 버그 수정, 내부 개선
   - 어떤 레벨로 올릴지 애매하면 진행하기 전에 사용자에게 확인한다
6. `package.json`의 `"version"` 필드를 새 버전으로 수정하고, 이 변경만 별도 커밋한다: `chore: bump version to vX.Y.Z`
7. push 전 원격 동기화 확인:
   - `git fetch` 후 로컬이 원격보다 뒤처져 있으면 `git pull`(merge)로 먼저 동기화한다
   - merge 중 충돌이 나면 중단하고 충돌 파일을 보여준 뒤 사용자 판단을 기다린다
8. `git push` (upstream 없으면 `-u origin <브랜치>`)
9. 버전을 갱신한 경우, 버전 커밋에 annotated 태그를 생성하고 push 한다:
   - `git tag -a vX.Y.Z -m "vX.Y.Z"`
   - `git push origin vX.Y.Z`
10. 커밋/pull/push/tag가 실패하면 중단하고 오류 원문을 그대로 보여준다. `--no-verify`나 강제 push로 임의 우회하지 않는다.

버전 갱신 여부·SemVer 레벨 판단이 애매한 경우를 제외하고는, 별도 확인 없이 바로 진행한다.
