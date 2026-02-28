using UnityEngine;

/// <summary>
/// 스테이지 출구 트리거.
/// 마지막 문 너머에 배치하는 BoxCollider(Is Trigger = true) 오브젝트에 붙인다.
/// 플레이어가 통과하면 GameManager.StageClear()를 호출한다.
///
/// 씬 배치 방법:
///   1. 빈 GameObject 생성 → "StageExit" 이름
///   2. BoxCollider 추가 → Is Trigger = true
///   3. 이 스크립트 추가
///   4. 마지막 문 바로 뒤에 위치 조정
/// </summary>
public class StageExitTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (GameManager.Instance == null)
        {
            Debug.LogError("[StageExitTrigger] GameManager 인스턴스를 찾을 수 없습니다!");
            return;
        }

        GameManager.Instance.StageClear();
    }
}
