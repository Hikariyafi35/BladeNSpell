using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilePlayer : MonoBehaviour
{
    public float damage = 1f;
    public float bulletSpeed = 10f;
    private Rigidbody2D rb;

    void Start()
    {
        Debug.Log("Bullet spawned at: " + transform.position);
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = transform.right * bulletSpeed;

        // Abaikan collider dengan player
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            Collider2D bulletCol = GetComponent<Collider2D>();
            Collider2D playerCol = player.GetComponent<Collider2D>();

            if (bulletCol != null && playerCol != null)
            {
                Physics2D.IgnoreCollision(bulletCol, playerCol);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Bullet hit: " + collision.gameObject.name);

        EnemyMelee enemyMelee = collision.gameObject.GetComponent<EnemyMelee>();
        EnemyRange enemyRange = collision.gameObject.GetComponent<EnemyRange>();
        EnemyGolem enemyGolem = collision.gameObject.GetComponent<EnemyGolem>();
        FireWorm fireWorm = collision.gameObject.GetComponent<FireWorm>();
        GoblinKing goblinKing = collision.gameObject.GetComponent<GoblinKing>();
        KingSlime kingSlime = collision.gameObject.GetComponent<KingSlime>();

        if (enemyMelee != null) enemyMelee.TakeDamage(damage);
        if (enemyRange != null) enemyRange.TakeDamage(damage);
        if (enemyGolem != null) enemyGolem.TakeDamage(damage);
        if (fireWorm != null) fireWorm.TakeDamage(damage);
        if (goblinKing != null) goblinKing.TakeDamage(damage);
        if (kingSlime != null) kingSlime.TakeDamage(damage);

        Destroy(gameObject);
    }
}