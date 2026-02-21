using UnityEngine;
using UnityEngine.Events;

public class HitTarget : MonoBehaviour
{
    // 큐브 하나당 필요한 피격 횟수 (기본 1발)
    [SerializeField] private int requiredHits = 1;

    // TargetSpawner가 코드로 구독하는 이벤트
    public UnityEvent onTargetCleared;

    private int _hitCount = 0;

    public void OnHit()
    {
        _hitCount++;
        Debug.Log($"[HitTarget] {_hitCount}/{requiredHits} 피격");

        if (_hitCount >= requiredHits)
        {
            onTargetCleared.Invoke();
            Destroy(gameObject);
        }
    }
}
