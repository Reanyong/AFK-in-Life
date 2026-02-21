using System.Collections;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    // 문이 열릴 때 올라가는 높이
    [SerializeField] private float openHeight = 3f;

    // 문이 열리고 닫히는 데 걸리는 시간 (초)
    [SerializeField] private float moveDuration = 1.5f;

    // 플레이어가 통과한 후 문이 닫히기까지 대기 시간 (초)
    [SerializeField] private float autoCloseDelay = 5f;

    private bool _isOpen = false;
    private Vector3 _closedPosition;
    private Coroutine _closeCoroutine;

    private void Awake()
    {
        // 시작 위치 = 닫힌 위치로 기억
        _closedPosition = transform.position;
    }

    public void Open()
    {
        if (_isOpen) return;
        _isOpen = true;

        Vector3 openPosition = _closedPosition + Vector3.up * openHeight;
        StartCoroutine(MoveCoroutine(transform.position, openPosition));
    }

    public void Close()
    {
        if (!_isOpen) return;
        _isOpen = false;
        StartCoroutine(MoveCoroutine(transform.position, _closedPosition));
    }

    // DoorPassTrigger에서 플레이어 통과 감지 시 호출
    public void OnPlayerPassed()
    {
        if (!_isOpen) return;

        // 이미 닫히는 중이면 취소하고 다시 시작
        if (_closeCoroutine != null) StopCoroutine(_closeCoroutine);
        _closeCoroutine = StartCoroutine(CloseAfterDelay());
    }

    private IEnumerator CloseAfterDelay()
    {
        Debug.Log($"[DoorController] {autoCloseDelay}초 후 문이 닫힙니다.");
        yield return new WaitForSeconds(autoCloseDelay);
        Close();
    }

    private IEnumerator MoveCoroutine(Vector3 from, Vector3 to)
    {
        float elapsed = 0f;

        while (elapsed < moveDuration)
        {
            elapsed += Time.deltaTime;
            // EaseOut: 끝에 가까울수록 천천히
            float t = 1f - Mathf.Pow(1f - (elapsed / moveDuration), 2f);
            transform.position = Vector3.Lerp(from, to, t);
            yield return null;
        }

        transform.position = to;
    }
}
