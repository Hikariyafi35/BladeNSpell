using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilePlayer : MonoBehaviour
{
    public float damage = 1f;  // Damage peluru

    // Fungsi untuk mendeteksi tabrakan dengan musuh menggunakan OnCollisionEnter2D
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Mengecek apakah peluru menyentuh musuh
        EnemyMelee enemyMelee = collision.gameObject.GetComponent<EnemyMelee>();
        EnemyRange enemyRange = collision.gameObject.GetComponent<EnemyRange>();
        EnemyGolem enemyGolem = collision.gameObject.GetComponent<EnemyGolem>();
        FireWorm fireWorm = collision.gameObject.GetComponent<FireWorm>();
        GoblinKing goblinKing = collision.gameObject.GetComponent<GoblinKing>();

        if (enemyMelee != null)
        {
            enemyMelee.TakeDamage(damage);  // Memberikan damage pada musuh melee
        }
        if (enemyRange != null)
        {
            enemyRange.TakeDamage(damage);  // Memberikan damage pada musuh range
        }
        if (enemyGolem != null)
        {
            enemyGolem.TakeDamage(damage);  // Memberikan damage pada EnemyGolem
        }
        if (fireWorm != null)
        {
            fireWorm.TakeDamage(damage);  // Memberikan damage pada FireWorm
        }
        if (goblinKing != null)
        {
            goblinKing.TakeDamage(damage);  // Memberikan damage pada GoblinKing
        }

        // Hancurkan peluru setelah mengenai musuh
        Destroy(gameObject);
    }
}