using UnityEngine;
using UnityEngine.UI;


public class Healthbar : MonoBehaviour
{
    public Character character;
    public Image healthBar;
    public Canvas gameOverCanvas;
    public float damageInterval = 1f; // Interval waktu untuk pengurangan health
    private float lastDamageTime; // Waktu saat terakhir health berkurang

    void Start()
    {
        // Set initial health
        character.currentHealth = character.maxHealth;
        UpdateHealthBar();
        if(gameOverCanvas != null){
            gameOverCanvas.gameObject.SetActive(false);
        }
    }

    // Function called when a collision occurs
    void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the collided object has the "Enemy" tag
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // Ambil komponen EnemyMelee dari musuh yang bertabrakan
            EnemyMelee enemyMelee = collision.gameObject.GetComponent<EnemyMelee>();
            EnemyRange enemyRange = collision.gameObject.GetComponent<EnemyRange>();
            if (enemyMelee != null)
            {
                TakeDamage(enemyMelee.damageCaused);  // Gunakan damageCaused dari musuh
                lastDamageTime = Time.time; // Catat waktu ketika pertama kali terkena damage
            }
            if(enemyRange != null){
                TakeDamage(enemyRange.damageCaused);
                lastDamageTime = Time.time;
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
                EnemyMelee enemyMelee = collision.gameObject.GetComponent<EnemyMelee>();
                EnemyRange enemyRange = collision.gameObject.GetComponent<EnemyRange>();
                if (enemyMelee != null)
                {
                    TakeDamage(enemyMelee.damageCaused);  // Gunakan damageCaused dari musuh
                    lastDamageTime = Time.time; // Reset waktu terakhir terkena damage
                }
                if(enemyRange != null) 
                {
                    TakeDamage(enemyRange.damageCaused);
                    lastDamageTime = Time.time;
                }
            }
        }
    }

    // Function to handle damage
    public void TakeDamage(int damage)
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
    public void AddHealth(int healthAmount){
        character.currentHealth += healthAmount;
        if (character.currentHealth > character.maxHealth){
            character.currentHealth = character.maxHealth;
        }
        UpdateHealthBar();
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
    AudioManager.Instance.musicSource.Stop();
    if (gameOverCanvas != null)
    {
        gameOverCanvas.gameObject.SetActive(true);
    }

    // Mengambil komponen Movement dan set kondisi mati
    Movement movement = GetComponent<Movement>();
    if (movement != null)
    {
        movement.SetDead(true); // Set isDead menjadi true
    }
}
}