using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public float damage = 1f;
    private void OnTriggerEnter2D(Collider2D collision) {
        EnemyMelee enemy = collision.gameObject.GetComponent<EnemyMelee>();
        if (enemy != null){
            enemy.TakeDamage(damage);
        }
    }
}
