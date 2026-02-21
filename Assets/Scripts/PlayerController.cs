using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // === 이동 관련 변수 ===
    public float moveSpeed = 5f;
    private CharacterController controller;

    // === 카메라 회전 관련 변수 ===
    public float mouseSensitivity = 100f; // 마우스 감도
    public Transform playerBody;         // 플레이어 오브젝트 (캡슐)
    public Transform cameraTransform;    // 카메라 오브젝트 (Main Camera)

    float xRotation = 0f; // 상하 회전 값 저장

    // === 총알 발사 관련 변수 ===
    public GameObject bulletPrefab;      // 총알 프리팹 (Inspector에서 연결)
    public Transform firePoint;          // 총알 발사 위치 (미리 만들어 둘 빈 오브젝트)
    public float bulletSpeed = 20f;
    public float fireRate = 0.5f;        // 0.5초에 한 발
    private float nextFireTime = 0f;     // 다음 발사 가능 시간


    void Start()
    {
        controller = GetComponent<CharacterController>();
        // 마우스 커서를 게임 화면 중앙에 고정하고 숨김
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // 1. === 이동 처리 ===
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 moveDirection = new Vector3(horizontal, 0f, vertical);
        moveDirection = transform.TransformDirection(moveDirection); // 로컬 좌표 기준으로 이동
        controller.Move(moveDirection * moveSpeed * Time.deltaTime);

        // 2. === 마우스 회전 (상하좌우) 처리 ===
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // 플레이어 좌우 회전 (y축)
        playerBody.Rotate(Vector3.up * mouseX);

        // 카메라 상하 회전 (x축)
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // 상하 90도 제한
        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // 3. === 총알 발사 처리 ===
        if (Input.GetButton("Fire1") && Time.time >= nextFireTime) // 마우스 왼쪽 클릭 + 쿨타임
        {
            nextFireTime = Time.time + fireRate; // 다음 발사 가능 시간 업데이트
            Shoot();
        }
    }

    void Shoot()
    {
        // 1. 발사 위치(firePoint)의 회전값을 가져옵니다.
        Quaternion bulletRotation = firePoint.rotation * Quaternion.Euler(90, 0, 0);
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, bulletRotation);

        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            // 핵심 수정: firePoint.forward 대신 cameraTransform.forward를 사용!
            // 이렇게 하면 카메라가 하늘을 보고 있으면 총알도 하늘로 날아갑니다.
            rb.linearVelocity = cameraTransform.forward * bulletSpeed;
        }
    }
}
