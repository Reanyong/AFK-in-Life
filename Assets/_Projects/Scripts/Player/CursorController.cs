using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// 커서 잠금 관리.
/// - 게임 시작 시 커서 숨김 + 화면 중앙 고정
/// - ESC 키: 커서 표시 / 게임 뷰 클릭: 다시 잠금
///
/// FPSPlayer 루트 오브젝트에 붙인다.
/// </summary>
public class CursorController : MonoBehaviour
{
    private void Start()
    {
        LockCursor();
    }

    private void Update()
    {
        // ESC → 커서 해제 (New Input System)
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            UnlockCursor();
            return;
        }

        // 커서 해제 상태에서 좌클릭 → 다시 잠금
        if (Cursor.lockState == CursorLockMode.None &&
            Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            LockCursor();
        }
    }

    private void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible   = false;
    }

    private void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible   = true;
    }
}
