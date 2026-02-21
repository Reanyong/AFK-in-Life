# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

<<<<<<< HEAD
**AFK-in-Life** is a 3D Unity game built with Unity 6 (6000.3.7f1) using the Universal Render Pipeline (URP). 1인칭 퍼즐 슈터 프로토타입. 현재 `dev_nyong` 브랜치에서 개발 중.

## Unity Development Commands

Unity 프로젝트는 CLI가 아닌 Unity Editor에서 빌드/실행한다.

- **프로젝트 열기**: Unity Hub에서 이 폴더를 로드하거나 `unity -projectPath /path/to/project`
- **테스트 실행**: Window > General > Test Runner (`com.unity.test-framework` 1.6.0)
- **빌드**: File > Build Settings
=======
**AFK-in-Life** is a 3D Unity game built with Unity 6 (6000.3.7f1) using the Universal Render Pipeline (URP). The project is in early development on the `dev_nyong` branch.

## Unity Development Commands

Unity projects are built and run through the Unity Editor, not via CLI. All build, test, and play operations are performed inside the editor. The following are relevant for development context:

- **Open project**: Open Unity Hub and load this folder, or use `unity -projectPath /path/to/project`
- **Run tests**: Use the Unity Test Runner (Window > General > Test Runner) — `com.unity.test-framework` 1.6.0 is installed
- **Build**: File > Build Settings inside the Unity Editor
>>>>>>> origin/dev_nyong

## Architecture

### Rendering
<<<<<<< HEAD
URP 17.3.0, PC/Mobile 분리 설정:
=======
URP 17.3.0 is used with separate PC and Mobile render pipeline configurations:
>>>>>>> origin/dev_nyong
- `Assets/Settings/PC_RPAsset.asset` / `PC_Renderer.asset`
- `Assets/Settings/Mobile_RPAsset.asset` / `Mobile_Renderer.asset`

### Input System
<<<<<<< HEAD
`com.unity.inputsystem` 1.18.0, **Send Messages / Broadcast Messages** 콜백 패턴 사용.
`PlayerController`가 메서드명(`OnMove`, `OnLook`, `OnFire`)으로 자동 수신.

- `Assets/InputSystem_Actions.inputactions` — 기본 Unity 스타터 에셋 (Look에 `<Mouse>/delta` 바인딩 추가됨)
- `Assets/_Projects/Input/PlayerInputActions.inputactions` — 프로젝트 전용

액션맵 `Player`: Move, Look, Fire, Interact, Crouch, Jump, Sprint, Previous, Next

### Player System (`Assets/_Projects/Scripts/Player/`)

**`PlayerController.cs`**
- `Awake()`: Rigidbody 획득, `CameraHolder`/`GunHoldPoint` 자식 Transform 탐색, 커서 Lock
- `Update()`: 마우스 X → `transform.Rotate(Y축)`, 마우스 Y → `cameraHolder.localRotation(X축)`, X 회전 ±80° 클램프
- `FixedUpdate()`: `Rigidbody.MovePosition`으로 이동, `transform.TransformDirection`으로 플레이어 방향 기준 이동
- `ShootProjectile()`: `gunHoldPoint`에서 `cameraHolder.forward` 방향으로 프로젝타일 스폰 후 `linearVelocity` 설정
- 필수 자식 오브젝트: `CameraHolder` (카메라 방향 기준), `GunHoldPoint` (총알 스폰 위치)

**`ProjectileLogic.cs`**
- `lifetime`초(기본 5s) 후 자폭, 또는 `OnTriggerEnter`에서:
  - `HitTarget` 컴포넌트가 있으면 `target.OnHit()` 호출 후 삭제
  - `"Ground"` 태그 콜라이더에 닿으면 삭제
- 프리팹 요구사항: `Rigidbody` + `Collider (Is Trigger = true)`

### Interaction System (`Assets/_Projects/Scripts/Interaction/`)

흐름: `TargetSpawner` 스폰 → 격파 → `HitTarget.onTargetCleared` → `TargetSpawner.OnOneTargetCleared()` → 전부 제거 시 `DoorController.Open()`

**`HitTarget.cs`**
- `requiredHits`(기본 1)번 맞으면 `onTargetCleared` UnityEvent 발동 후 `Destroy(gameObject)`
- `TargetSpawner`가 런타임에 `AddListener`로 구독

**`TargetSpawner.cs`**
- `targetCount`개 타겟을 `spawnHalfExtents` 범위 내 랜덤 스폰 (기본 4개, 반경 4×3)
- `minDistanceBetweenTargets`(기본 1.5f)로 겹침 방지, 최대 30번 재시도
- 모든 타겟 제거 시 `door.Open()` 호출

**`DoorController.cs`**
- `Open()`: `_closedPosition + Vector3.up * openHeight`로 EaseOut 슬라이드
- `OnPlayerPassed()`: `DoorPassTrigger`에서 호출, `autoCloseDelay`(기본 3s) 후 `Close()`
- `_closedPosition`을 `Awake()`에서 저장해 정확한 위치 복원

**`DoorPassTrigger.cs`**
- `BoxCollider (Is Trigger = true)`와 함께 사용
- `"Player"` 태그 감지 시 `door.OnPlayerPassed()` 호출

### Prefabs (`Assets/_Projects/Prefabs/`)
- `Projectile.prefab` — `Rigidbody` + `SphereCollider (Is Trigger = true)`, Rigidbody Constraints: 모든 회전 고정
- `Target.prefab` — `BoxCollider (Is Trigger = false)` + `HitTarget` 스크립트, Required Hits = 1

### Scene Structure
- `Assets/Scenes/SampleScene.unity` — 기본 Unity 스타터 씬
- `Assets/Scenes/Playground_dev.unity` — 현재 개발/테스트 씬

### Third-party Assets
- `Assets/KINEMATION/FPSAnimationPack/` — FPS 애니메이션 팩 (구매 에셋)
- `Assets/Low Poly Weapons VOL.1/` — 저폴리 무기 에셋 (구매 에셋, 보라색 머티리얼은 Window > Rendering > Render Pipeline Converter로 URP 변환)
=======
Uses Unity's new Input System (`com.unity.inputsystem` 1.18.0) with the **Send Messages / Broadcast Messages** callback pattern. `PlayerController` receives callbacks automatically via method names (`OnMove`, `OnLook`, `OnFire`).

Two input action assets exist:
- `Assets/InputSystem_Actions.inputactions` — default Unity starter asset
- `Assets/_Projects/Input/PlayerInputActions.inputactions` — project-specific input actions

Action map `Player` defines: Move, Look, Fire, Interact, Crouch, Jump, Sprint, Previous, Next. Supports Keyboard&Mouse, Gamepad, Touch, Joystick, and XR control schemes.

### Player System (`Assets/_Projects/Scripts/Player/`)
- **`PlayerController.cs`** — Attached to the player GameObject. Requires two child transforms: `CameraHolder` (camera direction for shooting) and `GunHoldPoint` (projectile spawn point). Movement uses `Rigidbody.MovePosition` in `FixedUpdate`; 2D input (x,y) maps to 3D XZ-plane movement. On `OnFire()`, instantiates a projectile prefab and sets its `Rigidbody.linearVelocity` in `CameraHolder.forward` direction.
- **`ProjectileLogic.cs`** — Attached to projectile prefabs. Self-destructs after `lifetime` seconds (default 5s) or on trigger with any collider tagged `"Ground"`.

### Prefabs (`Assets/_Projects/Prefabs/`)
- `Projectile.prefab` — Must have a `Rigidbody` and `Collider` (trigger) component; used by `PlayerController.ShootProjectile()`.

### Scene Structure
- `Assets/Scenes/SampleScene.unity` — Default Unity starter scene
- `Assets/Scenes/Playground_dev.unity` — Active development/test scene
>>>>>>> origin/dev_nyong

## Key Packages

| Package | Version | Purpose |
|---|---|---|
<<<<<<< HEAD
| `com.unity.render-pipelines.universal` | 17.3.0 | URP 렌더링 |
| `com.unity.inputsystem` | 1.18.0 | New Input System |
| `com.unity.ai.navigation` | 2.0.9 | NavMesh / AI 경로탐색 |
| `com.unity.timeline` | 1.8.10 | 타임라인 애니메이션 |
| `com.unity.multiplayer.center` | 1.0.1 | 멀티플레이어 설정 |
| `com.unity.test-framework` | 1.6.0 | 유닛/통합 테스트 |

## Conventions

- 프로젝트 스크립트: `Assets/_Projects/Scripts/`, 프리팹: `Assets/_Projects/Prefabs/`, 인풋: `Assets/_Projects/Input/`
- 에러 로그 접두사 패턴: `Debug.LogError("[ClassName] ...")`
- 주석은 한국어로 작성
- **Git 커밋 시 `Co-Authored-By: Claude` 워터마크를 추가하지 않는다.**
- 필수 태그: `"Ground"` (바닥), `"Player"` (플레이어) — Project Settings > Tags and Layers에서 등록
=======
| `com.unity.render-pipelines.universal` | 17.3.0 | URP rendering |
| `com.unity.inputsystem` | 1.18.0 | New Input System |
| `com.unity.ai.navigation` | 2.0.9 | NavMesh / AI pathfinding |
| `com.unity.timeline` | 1.8.10 | Timeline animation |
| `com.unity.multiplayer.center` | 1.0.1 | Multiplayer setup |
| `com.unity.test-framework` | 1.6.0 | Unit/integration tests |

## Conventions

- Project scripts live under `Assets/_Projects/Scripts/` (not `Assets/Scripts/`), prefabs under `Assets/_Projects/Prefabs/`, and input assets under `Assets/_Projects/Input/`.
- Error logging uses the `[ClassName]` prefix pattern: `Debug.LogError("[PlayerController] ...")`.
- Comments are written in Korean.
>>>>>>> origin/dev_nyong
