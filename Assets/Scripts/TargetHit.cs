using UnityEngine;

public class TargetHit : MonoBehaviour
{
    private MeshRenderer meshRenderer;
    public Color hitColor = Color.red; // 맞았을 때 변할 색상
    private Color originalColor;      // 원래 색상 저장용

    void Start()
    {
        // 구체의 겉면(MeshRenderer)을 가져와서 색상을 바꿀 준비를 합니다.
        meshRenderer = GetComponent<MeshRenderer>();
        originalColor = meshRenderer.material.color;
    }

    // 무언가와 충돌했을 때 실행되는 함수 (총알의 Collider가 Is Trigger인 경우)
    void OnTriggerEnter(Collider other)
    {
        // 충돌한 물체가 "Bullet" 태그를 가졌는지 확인
        if (other.CompareTag("Bullet") || other.gameObject.name.Contains("Bullet"))
        {
            ChangeColor();
        }
    }

    void ChangeColor()
    {
        // 색상을 지정한 색으로 바꿉니다.
        meshRenderer.material.color = hitColor;

        // (선택 사항) 0.2초 뒤에 다시 원래 색으로 돌리고 싶다면 아래 주석을 푸세요.
        // Invoke("RestoreColor", 0.2f);
    }

    void RestoreColor()
    {
        meshRenderer.material.color = originalColor;
    }
}