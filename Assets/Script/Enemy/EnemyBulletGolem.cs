using UnityEngine;

public class EnemyBulletGolem : MonoBehaviour
{
    public float bulletSpeed = 5f;  // Kecepatan peluru
    public int bulletDamage = 1;
    private Vector2 moveDirection;  // Arah gerakan peluru

    private void Start()
    {
        // Menghancurkan peluru setelah 5 detik agar tidak terus ada di game world
        Destroy(gameObject, 5f);
    }

    public void SetMoveDirection(Vector2 direction)
    {
        moveDirection = direction.normalized;  // Menormalisasi arah gerakan peluru
    }

    private void Update()
    {
        // Gerakkan peluru ke arah yang ditentukan
        transform.Translate(moveDirection * bulletSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Ambil komponen Healthbar dari Player
            Healthbar healthbar = collision.GetComponent<Healthbar>();

            if (healthbar != null)
            {
                // Player menerima damage melalui Healthbar.cs
                healthbar.TakeDamage(bulletDamage);
            }

            // Hancurkan peluru setelah mengenai player
            Destroy(this.gameObject);
        }
    }
}