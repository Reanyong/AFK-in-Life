using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    public GameObject bulletPrefab; // 
    public float bulletSpeed = 20f;

    void Update()
    {
        if (Input.GetButtonDown("Fire1")) // 
        {
            Shoot();
        }
    }

    void Shoot()
    {
        // 
        GameObject bullet = Instantiate(bulletPrefab, transform.position + transform.forward, Quaternion.identity);

        // 
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = transform.forward * bulletSpeed;
        }
    }
}