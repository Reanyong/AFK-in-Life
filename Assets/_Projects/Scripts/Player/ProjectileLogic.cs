using UnityEngine;

/// <summary>
/// 프로젝타일(총알) behavior.
/// - lifetime 초 후 자동 소멸
/// - 플레이어 태그를 제외한 모든 콜라이더에 닿으면 소멸
/// - HitTarget 컴포넌트가 있으면 OnHit() 호출 후 소멸
///
/// 프리팹 요구사항:
///   Rigidbody + Collider (Is Trigger = true)
///   Rigidbody Constraints: Freeze Rotation XYZ
/// </summary>
public class ProjectileLogic : MonoBehaviour
{
    [Tooltip("자동 소멸까지 생존 시간 (초)")]
    public float lifetime = 3f;

    private void Start()
    {
        // Update 루프 없이 일정 시간 후 소멸
        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter(Collider other)
    {
        // 발사한 플레이어 본인은 무시
        if (other.CompareTag("Player")) return;

        // 타겟에 맞으면 피격 처리
        HitTarget target = other.GetComponent<HitTarget>();
        if (target != null)
        {
            target.OnHit();
        }

        // 플레이어 외 무엇이든 닿으면 소멸 (벽, 바닥, 타겟 모두 포함)
        Destroy(gameObject);
    }
}
