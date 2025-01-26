using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBasicFireworm : MonoBehaviour
{
    public float speed;
    public int bulletDamage = 1; // Jumlah damage yang diberikan oleh peluru
    Rigidbody2D bulletRB;
    GameObject target;
    SpriteRenderer bulletSprite;
    Animator bulletAnimator; // Tambahkan referensi Animator
    Collider2D bulletCollider; // Tambahkan referensi Collider

    void Start()
    {
        bulletRB = GetComponent<Rigidbody2D>();
        target = GameObject.FindGameObjectWithTag("Player");
        bulletSprite = GetComponent<SpriteRenderer>(); // Ambil komponen SpriteRenderer
        bulletAnimator = GetComponent<Animator>(); // Ambil komponen Animator
        bulletCollider = GetComponent<Collider2D>(); // Ambil komponen Collider2D

        // Mengatur arah peluru menuju player
        Vector2 moveDir = (target.transform.position - transform.position).normalized * speed;
        bulletRB.velocity = new Vector2(moveDir.x, moveDir.y);

        // Flip sprite jika peluru bergerak ke kiri
        FlipSprite(moveDir.x);

        // Menghancurkan peluru setelah 2 detik jika tidak kena target
        Destroy(this.gameObject, 2f);
    }

    // Fungsi untuk membalik sprite peluru
    private void FlipSprite(float moveDirectionX)
    {
        if (moveDirectionX < 0)
        {
            // Membalik sprite jika peluru bergerak ke kiri
            bulletSprite.flipX = true;
        }
        else
        {
            // Tidak membalik sprite jika peluru bergerak ke kanan
            bulletSprite.flipX = false;
        }
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

            // Memutar animasi "Explode" di Animator
            bulletAnimator.SetTrigger("Explode");

            // Matikan collider peluru agar tidak terjadi tabrakan lagi
            bulletCollider.enabled = false;

            // Hancurkan peluru setelah animasi selesai
            Destroy(this.gameObject, 0.5f); // Tunggu beberapa detik agar animasi selesai
        }
    }
}