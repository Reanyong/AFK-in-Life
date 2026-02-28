using KINEMATION.FPSAnimationPack.Scripts.Player;
using KINEMATION.FPSAnimationPack.Scripts.Weapon;
using UnityEngine;

/// <summary>
/// FPSWeapon을 상속해 실제 프로젝타일 스폰을 추가하는 무기 클래스.
///
/// 프리팹 설정 방법:
///   1. MX16A4.prefab / M1911.prefab 열기
///   2. 기존 FPSWeapon 컴포넌트를 Remove하고 ProjectileWeapon 추가
///   3. Projectile Prefab 슬롯에 Projectile.prefab 연결
///   4. Projectile Speed: 80 권장
/// </summary>
public class ProjectileWeapon : FPSWeapon
{
    [Header("프로젝타일 설정")]
    [Tooltip("총구 위치 Transform. 없으면 카메라 앞에서 발사.")]
    [SerializeField] private Transform muzzlePoint;

    [Tooltip("발사할 프로젝타일 프리팹 (Projectile.prefab)")]
    [SerializeField] private GameObject projectilePrefab;

    [Tooltip("프로젝타일 초기 속도 (m/s). 80~120 권장.")]
    [SerializeField] private float projectileSpeed = 80f;

    // 달리는 도중 bob 없는 안정적인 조준 방향을 위해 FPSPlayer 참조
    private FPSPlayer _fpsPlayer;

    public override void Initialize(GameObject owner)
    {
        base.Initialize(owner);
        // SK_Arms_Mono에 붙어있는 FPSPlayer 컴포넌트 참조
        _fpsPlayer = owner.GetComponent<FPSPlayer>();
    }

    protected override void OnFire()
    {
        // 발사 불가 상태면 아무것도 하지 않음
        if (!_isFiring || _isReloading) return;

        // 달리는 중에는 발사 차단
        if (_fpsPlayer != null && _fpsPlayer.IsSprinting) return;

        // 탄약 없으면 자동 장전 후 종료 (총알 스폰 안 함)
        if (_activeAmmo <= 0)
        {
            OnReload();
            return;
        }

        // 기존 KINEMATION 발사 처리 (애니메이션, 사운드, 반동, 탄약 감소)
        base.OnFire();

        SpawnProjectile();
    }

    public override void OnReload()
    {
        // 이미 장전 중이면 무시 (R 여러 번 눌러도 한 번만 장전)
        if (_isReloading) return;
        base.OnReload();
    }

    private void SpawnProjectile()
    {
        if (projectilePrefab == null)
        {
            Debug.LogError("[ProjectileWeapon] Projectile Prefab이 설정되지 않았습니다!");
            return;
        }

        if (Camera.main == null)
        {
            Debug.LogError("[ProjectileWeapon] Camera.main을 찾을 수 없습니다!");
            return;
        }

        // 방향: FPSPlayer의 bob 없는 안정적인 조준 방향
        // FPSPlayer가 없으면 Camera.main.forward 폴백
        Vector3 direction = _fpsPlayer != null
            ? _fpsPlayer.StableAimDirection
            : Camera.main.transform.forward;

        // 위치: 카메라에서 살짝 앞 (화면 중앙 기준)
        Vector3 spawnPos = Camera.main.transform.position + direction * 0.5f;

        GameObject proj = Instantiate(projectilePrefab, spawnPos, Quaternion.LookRotation(direction));

        Rigidbody rb = proj.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = direction * projectileSpeed;
        }
    }
}
