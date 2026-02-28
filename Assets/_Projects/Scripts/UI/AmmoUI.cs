using KINEMATION.FPSAnimationPack.Scripts.Player;
using TMPro;
using UnityEngine;

/// <summary>
/// 현재 무기의 탄약 수와 재장전 상태를 HUD에 표시.
/// Canvas 하위 오브젝트에 붙인다.
/// </summary>
public class AmmoUI : MonoBehaviour
{
    [Tooltip("탄약 수 텍스트 (예: 30 / 30)")]
    [SerializeField] private TMP_Text ammoText;

    [Tooltip("장전 중 표시 텍스트 오브젝트")]
    [SerializeField] private TMP_Text reloadText;

    private FPSPlayer _fpsPlayer;

    private void Start()
    {
        _fpsPlayer = FindFirstObjectByType<FPSPlayer>();

        if (_fpsPlayer == null)
            Debug.LogWarning("[AmmoUI] FPSPlayer를 씬에서 찾지 못했습니다.");

        if (reloadText != null)
            reloadText.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (_fpsPlayer == null) return;

        var weapon = _fpsPlayer.GetActiveWeapon();
        if (weapon == null) return;

        // 탄약이 리셋됐으면 재장전 완료로 간주 (IsReloading 타이밍 보정)
        bool reloading = weapon.IsReloading && weapon.GetActiveAmmo() < weapon.GetMaxAmmo();

        // 재장전 중엔 ammoText 숨김 (겹침 방지)
        if (ammoText != null)
        {
            ammoText.gameObject.SetActive(!reloading);
            if (!reloading)
                ammoText.text = $"{weapon.GetActiveAmmo()} / {weapon.GetMaxAmmo()}";
        }

        if (reloadText != null)
            reloadText.gameObject.SetActive(reloading);
    }
}
