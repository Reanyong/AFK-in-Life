using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 게임 전체 흐름을 관리하는 싱글톤 매니저.
/// DontDestroyOnLoad로 씬 전환 후에도 유지된다.
///
/// 사용법:
///   - Stage01 씬의 GameManager 프리팹에 붙여둔다.
///   - StageClear() : 스테이지 클리어 시 호출 (StageExitTrigger에서 호출)
///   - GameOver()   : 플레이어 사망 시 호출 (나중에 PlayerHealth에서 호출)
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    // ── 게임 상태 ──────────────────────────────────────────
    public enum GameState { Playing, StageClear, GameOver }
    public GameState CurrentState { get; private set; } = GameState.Playing;

    // ── UI가 구독하는 정적 이벤트 ──────────────────────────
    public static event Action OnStageClearEvent;
    public static event Action OnGameOverEvent;

    // ── Inspector 설정 ─────────────────────────────────────
    [Tooltip("클리어 연출 후 다음 씬 로드까지 대기 시간 (초)")]
    [SerializeField] private float clearToNextSceneDelay = 3f;

    [Tooltip("게임오버 연출 후 씬 재시작까지 대기 시간 (초)")]
    [SerializeField] private float gameOverToRestartDelay = 3f;

    // ──────────────────────────────────────────────────────
    private void Awake()
    {
        // 싱글톤 — 중복 인스턴스 방지
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += HandleSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= HandleSceneLoaded;
    }

    /// <summary>
    /// 새 씬이 로드될 때마다 상태를 Playing으로 초기화.
    /// DontDestroyOnLoad 때문에 수동으로 리셋해야 함.
    /// </summary>
    private void HandleSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        CurrentState = GameState.Playing;
        Debug.Log($"[GameManager] 씬 로드됨: {scene.name} — 상태 초기화");
    }

    // ── 공개 API ────────────────────────────────────────────

    /// <summary>
    /// 스테이지 클리어. StageExitTrigger에서 호출.
    /// </summary>
    public void StageClear()
    {
        if (CurrentState != GameState.Playing) return;

        CurrentState = GameState.StageClear;
        Debug.Log("[GameManager] 스테이지 클리어!");
        OnStageClearEvent?.Invoke();
        StartCoroutine(LoadNextSceneRoutine());
    }

    /// <summary>
    /// 게임 오버. 플레이어 사망 시 호출 (미래 PlayerHealth에서 사용).
    /// </summary>
    public void GameOver()
    {
        if (CurrentState != GameState.Playing) return;

        CurrentState = GameState.GameOver;
        Debug.Log("[GameManager] 게임 오버!");
        OnGameOverEvent?.Invoke();
        StartCoroutine(RestartSceneRoutine());
    }

    // ── 코루틴 ──────────────────────────────────────────────

    private IEnumerator LoadNextSceneRoutine()
    {
        yield return new WaitForSeconds(clearToNextSceneDelay);

        int nextIndex = SceneManager.GetActiveScene().buildIndex + 1;

        // 마지막 씬을 넘어가면 0번(메인 메뉴 또는 엔딩)으로
        if (nextIndex >= SceneManager.sceneCountInBuildSettings)
        {
            Debug.Log("[GameManager] 마지막 스테이지 완료 — 씬 0으로 이동");
            SceneManager.LoadScene(0);
        }
        else
        {
            SceneManager.LoadScene(nextIndex);
        }
    }

    private IEnumerator RestartSceneRoutine()
    {
        yield return new WaitForSeconds(gameOverToRestartDelay);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
