using UnityEngine;

public class WeaponMelee : MonoBehaviour
{
    public float damage = 1f;

    private void OnTriggerEnter2D(Collider2D collision) {
        EnemyMelee enemyMelee = collision.GetComponent<EnemyMelee>();
        EnemyRange enemyRange= collision.GetComponent<EnemyRange>();
        EnemyGolem enemyGolem = collision.GetComponent<EnemyGolem>();
        if(enemyMelee != null) {
            enemyMelee.TakeDamage(damage);
        }
        if(enemyRange != null) {
            enemyRange.TakeDamage(damage);
        }
        if (enemyGolem != null) {
            enemyGolem.TakeDamage(damage);
        }
    }
}
