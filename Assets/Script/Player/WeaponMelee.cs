using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponMelee : MonoBehaviour
{
    public float damage = 1f;

    private void OnTriggerEnter2D(Collider2D collision) {
        EnemyMelee enemyMelee = collision.GetComponent<EnemyMelee>();
        if(enemyMelee != null) {
            enemyMelee.TakeDamage(damage);
        }
    }
}
