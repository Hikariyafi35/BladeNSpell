using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public float speed;
    public int bulletDamage = 1; // Jumlah damage yang diberikan oleh peluru
    Rigidbody2D bulletRB;
    GameObject target;

    void Start()
    {
        bulletRB = GetComponent<Rigidbody2D>();
        target = GameObject.FindGameObjectWithTag("Player");

        // Mengatur arah peluru menuju player
        Vector2 moveDir = (target.transform.position - transform.position).normalized * speed;
        bulletRB.velocity = new Vector2(moveDir.x, moveDir.y);

        // Menghancurkan peluru setelah 2 detik jika tidak kena target
        Destroy(this.gameObject, 2f);
    }

    // Jika peluru menyentuh player
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