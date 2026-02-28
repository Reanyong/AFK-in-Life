using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// 스테이지 클리어 / 게임오버 오버레이 UI 컨트롤러.
/// GameManager의 정적 이벤트를 구독해 패널을 표시한다.
///
/// Canvas 계층 구조 예시:
///   Canvas
///   └─ StageUIController (이 스크립트)
///      ├─ ClearPanel        (기본 비활성화)
///      │   └─ ClearText     (TextMeshProUGUI)
///      └─ GameOverPanel     (기본 비활성화)
///          └─ GameOverText  (TextMeshProUGUI)
/// </summary>
public class StageUIController : MonoBehaviour
{
    [Header("클리어 패널")]
    [SerializeField] private GameObject clearPanel;
    [SerializeField] private TextMeshProUGUI clearText;

    [Header("게임오버 패널")]
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private TextMeshProUGUI gameOverText;

    [Header("페이드 설정")]
    [Tooltip("패널이 페이드인 되는 시간 (초)")]
    [SerializeField] private float fadeDuration = 0.5f;

    // ──────────────────────────────────────────────────────
    private void OnEnable()
    {
        GameManager.OnStageClearEvent += ShowClearUI;
        GameManager.OnGameOverEvent   += ShowGameOverUI;
    }

    private void OnDisable()
    {
        GameManager.OnStageClearEvent -= ShowClearUI;
        GameManager.OnGameOverEvent   -= ShowGameOverUI;
    }

    private void Start()
    {
        // 시작 시 모두 숨김
        if (clearPanel != null)    clearPanel.SetActive(false);
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
    }

    // ── 이벤트 핸들러 ────────────────────────────────────────

    private void ShowClearUI()
    {
        if (clearPanel == null) return;
        clearPanel.SetActive(true);

        if (clearText != null)
            clearText.text = "STAGE CLEAR";

        StartCoroutine(FadeInPanel(clearPanel));
    }

    private void ShowGameOverUI()
    {
        if (gameOverPanel == null) return;
        gameOverPanel.SetActive(true);

        if (gameOverText != null)
            gameOverText.text = "GAME OVER";

        StartCoroutine(FadeInPanel(gameOverPanel));
    }

    // ── 페이드 코루틴 ────────────────────────────────────────

    /// <summary>
    /// 패널 루트의 CanvasGroup alpha를 0 → 1로 페이드.
    /// CanvasGroup이 없으면 그냥 바로 표시.
    /// </summary>
    private IEnumerator FadeInPanel(GameObject panel)
    {
        CanvasGroup cg = panel.GetComponent<CanvasGroup>();
        if (cg == null)
        {
            // CanvasGroup 없으면 그냥 표시
            yield break;
        }

        cg.alpha = 0f;
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            cg.alpha = Mathf.Clamp01(elapsed / fadeDuration);
            yield return null;
        }

        cg.alpha = 1f;
    }
}
