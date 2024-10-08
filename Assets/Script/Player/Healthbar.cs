using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Healthbar : MonoBehaviour
{
    public Character character;
    public Image healthBar;
    public float damageInterval = 1f; // Interval waktu untuk pengurangan health
    private float lastDamageTime; // Waktu saat terakhir health berkurang

    void Start()
    {
        // Set initial health

        UpdateHealthBar();
    }

    // Function called when a collision occurs
    void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the collided object has the "Enemy" tag
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // Ambil komponen EnemyMelee dari musuh yang bertabrakan
            EnemyMelee enemy = collision.gameObject.GetComponent<EnemyMelee>();
            
            if (enemy != null)
            {
                TakeDamage(enemy.damageCaused);  // Gunakan damageCaused dari musuh
                lastDamageTime = Time.time; // Catat waktu ketika pertama kali terkena damage
            }
        }
    }

    // Function called while staying in collision
    void OnCollisionStay2D(Collision2D collision)
    {
        // Check if the object we're still colliding with is tagged as "Enemy"
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // Cek apakah waktu yang telah berlalu sejak terakhir kali terkena damage melebihi damageInterval
            if (Time.time - lastDamageTime >= damageInterval)
            {
                // Ambil komponen EnemyMelee dari musuh yang bertabrakan
                EnemyMelee enemy = collision.gameObject.GetComponent<EnemyMelee>();
                
                if (enemy != null)
                {
                    TakeDamage(enemy.damageCaused);  // Gunakan damageCaused dari musuh
                    lastDamageTime = Time.time; // Reset waktu terakhir terkena damage
                }
            }
        }
    }

    // Function to handle damage
    void TakeDamage(int damage)
    {
        character.currentHealth -= damage;

        // Prevent health from going below 0
        if (character.currentHealth <= 0)
        {
            character.currentHealth = 0;
            Die();  // Call death function if health reaches 0
        }

        UpdateHealthBar();
        Debug.Log("Current Health: " + character.currentHealth);
    }

    // Function to update the health bar
    void UpdateHealthBar()
    {
        float fillAmount = (float)character.currentHealth / character.maxHealth;
        healthBar.fillAmount = fillAmount;
    }

    // Function called when health reaches 0
    void Die()
    {
        Debug.Log("Player Died");
        // Handle player death here (e.g., restart level, show game over screen)
    }
}