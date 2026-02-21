using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;

    // 마우스 감도
    [SerializeField] private float mouseSensitivity = 0.1f;

    // 총알 발사 관련
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float projectileSpeed = 20f;

    private Rigidbody _rb;
    [SerializeField] private Transform cameraHolder;
    [SerializeField] private Transform gunHoldPoint;

    private Vector2 _moveInput;
    private Vector2 _lookInput;
    private float _xRotation = 0f;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        cameraHolder = transform.Find("CameraHolder");
        gunHoldPoint = transform.Find("GunHoldPoint");

        if (cameraHolder == null)
            Debug.LogError("[PlayerController] CameraHolder를 찾을 수 없습니다!");

        if (gunHoldPoint == null)
            Debug.LogError("[PlayerController] GunHoldPoint를 찾을 수 없습니다!");

        // 커서 숨기기 및 화면 중앙에 고정
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void OnMove(InputValue value)
    {
        _moveInput = value.Get<Vector2>();
    }

    public void OnLook(InputValue value)
    {
        _lookInput = value.Get<Vector2>();
    }

    public void OnFire()
    {
        ShootProjectile();
    }

    private void Update()
    {
        // 마우스 좌우 → 플레이어 Y축 회전 (몸통이 돌아감)
        transform.Rotate(Vector3.up * (_lookInput.x * mouseSensitivity));

        // 마우스 위아래 → CameraHolder X축 회전 (고개를 들고 내림)
        _xRotation -= _lookInput.y * mouseSensitivity;
        _xRotation = Mathf.Clamp(_xRotation, -80f, 80f); // 너무 꺾이지 않게 제한
        cameraHolder.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);
    }

    private void FixedUpdate()
    {
        if (_moveInput == Vector2.zero) return;

        // 2D 입력(x, y)을 3D 공간의 (x, z)로 변환 (y는 위아래이므로 0)
        Vector3 moveDir = new Vector3(_moveInput.x, 0, _moveInput.y).normalized;

        // 플레이어가 바라보는 방향 기준으로 이동
        Vector3 worldMove = transform.TransformDirection(moveDir);
        Vector3 targetPosition = _rb.position + worldMove * (moveSpeed * Time.fixedDeltaTime);

        _rb.MovePosition(targetPosition);
    }

    private void ShootProjectile()
    {
        if (gunHoldPoint == null || projectilePrefab == null)
        {
            Debug.LogError("[PlayerController] 총알 발사 실패!");
            return;
        }

        Vector3 spawnPosition = gunHoldPoint.position;

        // 발사 시점의 카메라 방향을 즉시 캡처
        Vector3 shootDirection = cameraHolder.forward.normalized;

        GameObject newProjectile = Instantiate(projectilePrefab, spawnPosition, Quaternion.identity);

        Rigidbody projectileRb = newProjectile.GetComponent<Rigidbody>();
        if (projectileRb != null)
        {
            projectileRb.linearVelocity = shootDirection * projectileSpeed;
        }
    }
}
