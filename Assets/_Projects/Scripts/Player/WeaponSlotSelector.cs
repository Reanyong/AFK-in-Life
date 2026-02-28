using KINEMATION.FPSAnimationPack.Scripts.Player;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// 1번/2번 키로 무기 슬롯을 직접 선택하는 컴포넌트.
/// FPSPlayer와 같은 GameObject에 붙인다.
///
/// PlayerInputActions 설정:
///   Player 액션맵의 "Previous" → 1번 키 바인딩
///   Player 액션맵의 "Next"     → 2번 키 바인딩
///   (또는 아래 OnSlot1 / OnSlot2를 직접 추가 바인딩)
/// </summary>
public class WeaponSlotSelector : MonoBehaviour
{
    private FPSPlayer _fpsPlayer;

    private void Awake()
    {
        _fpsPlayer = GetComponent<FPSPlayer>();
        if (_fpsPlayer == null)
            Debug.LogError("[WeaponSlotSelector] FPSPlayer 컴포넌트를 찾을 수 없습니다!");
    }

    // Input System Send Messages — PlayerInputActions에 "Slot1" 액션 추가 후 바인딩: [1]
    public void OnSlot1(InputValue value)
    {
        if (value.isPressed) _fpsPlayer?.SelectWeaponByIndex(0);
    }

    // Input System Send Messages — PlayerInputActions에 "Slot2" 액션 추가 후 바인딩: [2]
    public void OnSlot2(InputValue value)
    {
        if (value.isPressed) _fpsPlayer?.SelectWeaponByIndex(1);
    }
}
