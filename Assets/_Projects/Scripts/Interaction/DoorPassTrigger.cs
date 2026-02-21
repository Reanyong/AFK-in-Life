using UnityEngine;

// 문 반대편에 배치하는 통과 감지용 트리거
// BoxCollider (Is Trigger = true) 와 함께 사용
public class DoorPassTrigger : MonoBehaviour
{
    [SerializeField] private DoorController door;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            door.OnPlayerPassed();
        }
    }
}
