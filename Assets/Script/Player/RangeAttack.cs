using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeAttack : MonoBehaviour
{
    public Transform player; // drag player ke sini dari Inspector
    public Vector3 offsetFromPlayer = Vector3.zero; // kalau mau geser sedikit posisi weapon

    public GameObject weaponObject;
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float bulletSpeed = 10f;
    private bool isAiming = false;
     public AudioClip shootSFX;  // SFX untuk tembakan
    private AudioSource audioSource;  // AudioSource untuk memainkan SFX

    void Start()
    {
        // Ambil komponen AudioSource di objek yang sama
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (player != null)
    {
        transform.position = player.position + offsetFromPlayer;
    }
        HandleAiming();
        HandleShooting();
        
    }

    void HandleAiming()
    {
        if (Input.GetMouseButtonDown(1)) // Klik kanan mulai
        {
            isAiming = true;
            weaponObject.SetActive(true);
        }

        if (Input.GetMouseButton(1) && isAiming)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 direction = mousePos - player.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            weaponObject.transform.rotation = Quaternion.Euler(0, 0, angle);

            Vector3 scale = weaponObject.transform.localScale;
            weaponObject.transform.rotation = Quaternion.Euler(0, 0, angle);
            weaponObject.transform.localScale = new Vector3(scale.x, Mathf.Abs(scale.y), scale.z);
        }
    }

    void HandleShooting()
    {
        if (Input.GetMouseButtonUp(1) && isAiming)
        {
            isAiming = false;
            weaponObject.SetActive(false);

            // Tembakkan peluru
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            Vector2 fireDirection = weaponObject.transform.right; // arah horizontal dari rotasi

            bullet.GetComponent<Rigidbody2D>().velocity = fireDirection * bulletSpeed;
            // Mainkan SFX saat tembakan
            if (audioSource != null && shootSFX != null)
            {
                audioSource.PlayOneShot(shootSFX);  // Memainkan SFX tembakan
            }
        }
    }
}
