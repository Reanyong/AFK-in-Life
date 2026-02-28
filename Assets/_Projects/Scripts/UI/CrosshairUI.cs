using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 화면 정중앙에 크로스헤어를 코드로 생성하는 컴포넌트.
/// 별도 스프라이트 없이 Unity UI Image로 그림.
///
/// 씬 설정:
///   Canvas (Screen Space - Overlay) 오브젝트에 이 스크립트 추가.
///   Play 시 자동으로 크로스헤어 생성.
/// </summary>
public class CrosshairUI : MonoBehaviour
{
    [Header("크로스헤어 모양")]
    [SerializeField] private Color color = new Color(1f, 1f, 1f, 0.9f);

    [Tooltip("중앙 점 크기 (px)")]
    [SerializeField] private float dotSize = 4f;

    [Tooltip("십자선 길이 (px)")]
    [SerializeField] private float lineLength = 12f;

    [Tooltip("십자선 두께 (px)")]
    [SerializeField] private float lineWidth = 2f;

    [Tooltip("중앙에서 십자선까지 간격 (px)")]
    [SerializeField] private float gap = 5f;

    private void Start()
    {
        BuildCrosshair();
    }

    private void BuildCrosshair()
    {
        // 중앙 점
        CreateRect("Dot", Vector2.zero, new Vector2(dotSize, dotSize));

        // 상하좌우 선
        float offset = gap + lineLength * 0.5f;
        CreateRect("Right", new Vector2( offset, 0f), new Vector2(lineLength, lineWidth));
        CreateRect("Left",  new Vector2(-offset, 0f), new Vector2(lineLength, lineWidth));
        CreateRect("Up",    new Vector2(0f,  offset), new Vector2(lineWidth, lineLength));
        CreateRect("Down",  new Vector2(0f, -offset), new Vector2(lineWidth, lineLength));
    }

    private void CreateRect(string objName, Vector2 anchoredPos, Vector2 size)
    {
        GameObject go = new GameObject(objName);
        go.transform.SetParent(transform, false);

        RectTransform rect = go.AddComponent<RectTransform>();
        rect.anchorMin        = Vector2.one * 0.5f;
        rect.anchorMax        = Vector2.one * 0.5f;
        rect.pivot            = Vector2.one * 0.5f;
        rect.anchoredPosition = anchoredPos;
        rect.sizeDelta        = size;

        Image img  = go.AddComponent<Image>();
        img.color  = color;
        img.raycastTarget = false; // UI 클릭 방해 방지
    }
}
