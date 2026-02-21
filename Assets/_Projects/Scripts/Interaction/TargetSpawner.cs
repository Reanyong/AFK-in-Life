using UnityEngine;

public class TargetSpawner : MonoBehaviour
{
    [SerializeField] private GameObject targetPrefab;
    [SerializeField] private DoorController door;

    // 스폰할 큐브 수
    [SerializeField] private int targetCount = 4;

    // 이 오브젝트 기준 스폰 범위 (X: 좌우, Y: 앞뒤)
    [SerializeField] private Vector2 spawnHalfExtents = new Vector2(2f, 1f);

    private int _remainingTargets;

    private void Start()
    {
        SpawnTargets();
    }

    private void SpawnTargets()
    {
        _remainingTargets = targetCount;

        for (int i = 0; i < targetCount; i++)
        {
            Vector3 offset = new Vector3(
                Random.Range(-spawnHalfExtents.x, spawnHalfExtents.x),
                0.5f, // 바닥 위에 살짝 띄움
                Random.Range(-spawnHalfExtents.y, spawnHalfExtents.y)
            );

            GameObject obj = Instantiate(targetPrefab, transform.position + offset, Quaternion.identity);
            obj.GetComponent<HitTarget>().onTargetCleared.AddListener(OnOneTargetCleared);
        }
    }

    private void OnOneTargetCleared()
    {
        _remainingTargets--;
        Debug.Log($"[TargetSpawner] 남은 타겟: {_remainingTargets}");

        if (_remainingTargets <= 0)
        {
            door.Open();
        }
    }
}
