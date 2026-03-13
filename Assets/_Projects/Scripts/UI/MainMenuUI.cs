using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// 메인 메뉴 UI 관리.
/// - 플레이어 이름 입력
/// - 방 이름 입력
/// - 게임 시작 / 설정 / 종료
/// </summary>
public class MainMenuUI : MonoBehaviour
{
    [Header("패널")]
    [SerializeField] private GameObject mainPanel;
    [SerializeField] private GameObject settingsPanel;

    [Header("입력")]
    [SerializeField] private TMP_InputField playerNameInput;
    [SerializeField] private TMP_InputField roomNameInput;

    [Header("버튼")]
    [SerializeField] private Button createRoomButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private Button settingsBackButton;

    [Header("씬")]
    [SerializeField] private string gameSceneName = "Playground_dev";

    private const string PlayerNameKey = "PlayerName";

    private void Start()
    {
        // 이전에 입력한 이름 불러오기
        if (playerNameInput != null)
            playerNameInput.text = PlayerPrefs.GetString(PlayerNameKey, "");

        createRoomButton?.onClick.AddListener(OnCreateRoom);
        settingsButton?.onClick.AddListener(OnSettings);
        quitButton?.onClick.AddListener(OnQuit);
        settingsBackButton?.onClick.AddListener(OnSettingsBack);

        ShowMain();
    }

    // ── 메인 패널 ──────────────────────────────────────────
    private void OnCreateRoom()
    {
        string playerName = playerNameInput != null ? playerNameInput.text.Trim() : "";
        string roomName   = roomNameInput   != null ? roomNameInput.text.Trim()   : "";

        if (string.IsNullOrEmpty(playerName))
        {
            Debug.LogWarning("[MainMenuUI] 플레이어 이름을 입력하세요.");
            return;
        }

        // 이름 저장
        PlayerPrefs.SetString(PlayerNameKey, playerName);
        PlayerPrefs.Save();

        // TODO: Mirror 연동 후 실제 방 생성으로 교체
        SceneManager.LoadScene(gameSceneName);
    }

    private void OnSettings()
    {
        mainPanel?.SetActive(false);
        settingsPanel?.SetActive(true);
    }

    private void OnQuit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    // ── 설정 패널 ──────────────────────────────────────────
    private void OnSettingsBack()
    {
        ShowMain();
    }

    private void ShowMain()
    {
        mainPanel?.SetActive(true);
        settingsPanel?.SetActive(false);
    }
}
