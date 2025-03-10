using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thunder : MonoBehaviour
{
    public float damage = 1f;

    private void OnTriggerEnter2D(Collider2D collision) {
        EnemyMelee enemyMelee = collision.GetComponent<EnemyMelee>();
        EnemyRange enemyRange= collision.GetComponent<EnemyRange>();
        EnemyGolem enemyGolem = collision.GetComponent<EnemyGolem>();
        FireWorm fireWorm= collision.GetComponent<FireWorm>();
        GoblinKing goblinKing= collision.GetComponent<GoblinKing>();
        if(enemyMelee != null) {
            enemyMelee.TakeDamage(damage);
        }
        if(enemyRange != null) {
            enemyRange.TakeDamage(damage);
        }
        if (enemyGolem != null) {
            enemyGolem.TakeDamage(damage);
        }
        if (fireWorm != null) {
            fireWorm.TakeDamage(damage);
        }
        if (goblinKing != null){
            goblinKing.TakeDamage(damage);
        }
    }
}
