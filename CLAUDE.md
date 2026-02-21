# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

**AFK-in-Life** is a 3D Unity game built with Unity 6 (6000.3.7f1) using the Universal Render Pipeline (URP). The project is in early development on the `dev_nyong` branch.

## Unity Development Commands

Unity projects are built and run through the Unity Editor, not via CLI. All build, test, and play operations are performed inside the editor. The following are relevant for development context:

- **Open project**: Open Unity Hub and load this folder, or use `unity -projectPath /path/to/project`
- **Run tests**: Use the Unity Test Runner (Window > General > Test Runner) — `com.unity.test-framework` 1.6.0 is installed
- **Build**: File > Build Settings inside the Unity Editor

## Architecture

### Rendering
URP 17.3.0 is used with separate PC and Mobile render pipeline configurations:
- `Assets/Settings/PC_RPAsset.asset` / `PC_Renderer.asset`
- `Assets/Settings/Mobile_RPAsset.asset` / `Mobile_Renderer.asset`

### Input System
Uses Unity's new Input System (`com.unity.inputsystem` 1.18.0) with the **Send Messages / Broadcast Messages** callback pattern. `PlayerController` receives callbacks automatically via method names (`OnMove`, `OnLook`, `OnFire`).

Two input action assets exist:
- `Assets/InputSystem_Actions.inputactions` — default Unity starter asset
- `Assets/_Projects/Input/PlayerInputActions.inputactions` — project-specific input actions

Action map `Player` defines: Move, Look, Fire, Interact, Crouch, Jump, Sprint, Previous, Next. Supports Keyboard&Mouse, Gamepad, Touch, Joystick, and XR control schemes.

### Player System (`Assets/_Projects/Scripts/Player/`)
- **`PlayerController.cs`** — Attached to the player GameObject. Requires two child transforms: `CameraHolder` (camera direction for shooting) and `GunHoldPoint` (projectile spawn point). Movement uses `Rigidbody.MovePosition` in `FixedUpdate`; 2D input (x,y) maps to 3D XZ-plane movement. On `OnFire()`, instantiates a projectile prefab and sets its `Rigidbody.linearVelocity` in `CameraHolder.forward` direction.
- **`ProjectileLogic.cs`** — Attached to projectile prefabs. Self-destructs after `lifetime` seconds (default 5s) or on trigger with any collider tagged `"Ground"`.

### Interaction System (`Assets/_Projects/Scripts/Interaction/`)
- **`HitTarget.cs`** — 피격 횟수 카운트, 기준 도달 시 UnityEvent 발동 후 자기 자신 삭제
- **`TargetSpawner.cs`** — 지정 구역에 타겟 N개 랜덤 스폰, 전부 제거 시 DoorController.Open() 호출
- **`DoorController.cs`** — 문 위로 슬라이드 Open/Close (EaseOut), 플레이어 통과 후 자동 닫힘
- **`DoorPassTrigger.cs`** — 플레이어 통과 감지 트리거, DoorController.OnPlayerPassed() 호출

### Prefabs (`Assets/_Projects/Prefabs/`)
- `Projectile.prefab` — Rigidbody + SphereCollider (Is Trigger = true)
- `Target.prefab` — BoxCollider (Is Trigger = false) + HitTarget 스크립트

### Scene Structure
- `Assets/Scenes/SampleScene.unity` — Default Unity starter scene
- `Assets/Scenes/Playground_dev.unity` — Active development/test scene

## Key Packages

| Package | Version | Purpose |
|---|---|---|
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
- **Git 커밋 시 `Co-Authored-By: Claude` 워터마크를 추가하지 않는다.**
