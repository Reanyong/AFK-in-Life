using UnityEngine;

public class Bullet : MonoBehaviour
{
    private bool isStuck = false;

    void OnTriggerEnter(Collider other)
    {
        // 이미 어딘가 박혔다면 무시
        if (isStuck) return;

        if (other.CompareTag("Target"))
        {
            isStuck = true;

            // 맞은 대상에 TargetHit 스크립트가 있다면 색 변화 함수를 실행시킴
            TargetHit target = other.GetComponent<TargetHit>();
            if (target != null)
            {
                target.Invoke("ChangeColor", 0); // 즉시 색 변경 실행
            }

            // 물리 멈추고 고정
            Rigidbody rb = GetComponent<Rigidbody>();
            rb.linearVelocity = Vector3.zero;
            rb.isKinematic = true;
            transform.SetParent(other.transform);
        }
    }
}