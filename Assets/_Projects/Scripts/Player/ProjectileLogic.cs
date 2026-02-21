using UnityEngine;

public class ProjectileLogic : MonoBehaviour
{
    // 총알이 생존할 시간 (초)
    public float lifetime = 5f;
    
    // 생성 시간을 기록하는 변수
    private float spawnTime;
    
    void Start()
    {
        // 이 총알이 생성되는 시점의 시간을 기록
        spawnTime = Time.time;
    }
    
    void Update()
    {
        // 현재 시간 - 생성 시간 = 경과 시간
        float elapsedTime = Time.time - spawnTime;
        
        // 만약 경과 시간이 lifetime (5초)보다 크면
        if (elapsedTime > lifetime)
        {
            // 이 게임 오브젝트(총알) 삭제
            Destroy(gameObject);
        }
    }
    
    // 충돌했을 때 호출되는 함수
    void OnTriggerEnter(Collider other)
    {
        // 타겟에 맞으면 피격 처리 후 총알 삭제
        HitTarget target = other.GetComponent<HitTarget>();
        if (target != null)
        {
            target.OnHit();
            Destroy(gameObject);
            return;
        }

        // Ground에 맞으면 총알 삭제
        if (other.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }
    }
}