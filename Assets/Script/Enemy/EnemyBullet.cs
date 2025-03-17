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
            // Ambil komponen Healthbar dan Character dari Player
            Healthbar healthbar = collision.GetComponent<Healthbar>();
            Character character = collision.GetComponent<Character>(); // Mendapatkan komponen Character

            if (healthbar != null && character != null)
            {
                // Mengecek apakah shield masih aktif
                if (character.currentShield > 0)
                {
                    // Jika shield aktif, kurangi shield terlebih dahulu
                    int shieldDamage = Mathf.Min(bulletDamage, character.currentShield); // Kurangi shield sebanyak mungkin
                    character.ReduceShield(shieldDamage); // Mengurangi shield
                    bulletDamage -= shieldDamage; // Kurangi damage yang masih tersisa setelah mengurangi shield
                    Debug.Log("Shield absorbed damage: " + shieldDamage);
                }

                // Setelah shield habis, atau jika shield habis, baru health yang berkurang
                if (bulletDamage > 0)
                {
                    healthbar.TakeDamage(bulletDamage); // Mengurangi health setelah shield habis
                    Debug.Log("Health reduced by: " + bulletDamage);
                }
            }

            // Hancurkan peluru setelah mengenai player
            Destroy(this.gameObject);
        }
    }
}