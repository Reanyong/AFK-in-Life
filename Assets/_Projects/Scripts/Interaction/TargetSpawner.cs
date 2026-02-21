using System.Collections.Generic;
using UnityEngine;

public class TargetSpawner : MonoBehaviour
{
    [SerializeField] private GameObject targetPrefab;
    [SerializeField] private DoorController door;

    // 스폰할 큐브 수
    [SerializeField] private int targetCount = 4;

    // 이 오브젝트 기준 스폰 범위 (X: 좌우, Y: 앞뒤)
    [SerializeField] private Vector2 spawnHalfExtents = new Vector2(4f, 3f);

    // 타겟 간 최소 거리
    [SerializeField] private float minDistanceBetweenTargets = 1.5f;

    private int _remainingTargets;

    private void Start()
    {
        SpawnTargets();
    }

    private void SpawnTargets()
    {
        _remainingTargets = targetCount;

        List<Vector3> spawnedPositions = new List<Vector3>();

        for (int i = 0; i < targetCount; i++)
        {
            Vector3 spawnPos = GetValidSpawnPosition(spawnedPositions);
            spawnedPositions.Add(spawnPos);

            GameObject obj = Instantiate(targetPrefab, spawnPos, Quaternion.identity);
            obj.GetComponent<HitTarget>().onTargetCleared.AddListener(OnOneTargetCleared);
        }
    }

    private Vector3 GetValidSpawnPosition(List<Vector3> existingPositions)
    {
        // 최대 30번 시도 후 그냥 배치 (무한루프 방지)
        for (int attempt = 0; attempt < 30; attempt++)
        {
            Vector3 candidate = transform.position + new Vector3(
                Random.Range(-spawnHalfExtents.x, spawnHalfExtents.x),
                0.5f,
                Random.Range(-spawnHalfExtents.y, spawnHalfExtents.y)
            );

            if (IsFarEnough(candidate, existingPositions))
                return candidate;
        }

        // 30번 다 실패하면 마지막 후보 그냥 반환
        return transform.position + new Vector3(
            Random.Range(-spawnHalfExtents.x, spawnHalfExtents.x),
            0.5f,
            Random.Range(-spawnHalfExtents.y, spawnHalfExtents.y)
        );
    }

    private bool IsFarEnough(Vector3 candidate, List<Vector3> existingPositions)
    {
        foreach (Vector3 pos in existingPositions)
        {
            if (Vector3.Distance(candidate, pos) < minDistanceBetweenTargets)
                return false;
        }
        return true;
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
