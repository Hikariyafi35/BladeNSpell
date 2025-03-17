using UnityEngine;
using UnityEngine.UI;


public class Healthbar : MonoBehaviour
{
    public Character character;
    public Image healthBar;
    public Image shieldBar;
    public Canvas gameOverCanvas;
    public float damageInterval = 1f; // Interval waktu untuk pengurangan health
    private float lastDamageTime; // Waktu saat terakhir health berkurang

    void Start()
    {
        // Set initial health
        character.currentHealth = character.maxHealth;
        UpdateHealthBar();
        UpdateShieldBar();
        shieldBar.fillAmount = 0;
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
                HandleDamage(enemyMelee.damageCaused);  // Gunakan damageCaused dari musuh
                lastDamageTime = Time.time; // Catat waktu ketika pertama kali terkena damage
            }
            if(enemyRange != null){
                HandleDamage(enemyRange.damageCaused);
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
                FireWorm fireWorm = collision.gameObject.GetComponent<FireWorm>();
                GoblinKing goblinKing = collision.gameObject.GetComponent<GoblinKing>();
                if (enemyMelee != null)
                {
                    HandleDamage(enemyMelee.damageCaused);  // Gunakan damageCaused dari musuh
                    lastDamageTime = Time.time; // Reset waktu terakhir terkena damage
                }
                if(enemyRange != null) 
                {
                    HandleDamage(enemyRange.damageCaused);
                    lastDamageTime = Time.time;
                }
                if(fireWorm != null)
                {
                    HandleDamage(fireWorm.damageCaused);
                    lastDamageTime = Time.time;
                }
                if(goblinKing != null)
                {
                    HandleDamage(goblinKing.damageCaused);
                    lastDamageTime = Time.time;
                }
            }
        }
    }

    // Function to handle damage
    void HandleDamage(int damage)
    {
        if (character.currentShield > 0)
        {
            // Jika shield aktif, kurangi shield terlebih dahulu
            int shieldDamage = Mathf.Min(damage, character.currentShield); // Kurangi shield sebanyak mungkin, sisanya ke health
            character.ReduceShield(shieldDamage);
            damage -= shieldDamage; // Sisa damage setelah mengurangi shield

            Debug.Log("Shield absorbed damage: " + shieldDamage);
        }

        if (damage > 0)
        {
            // Jika masih ada damage yang tersisa, kurangi health
            TakeDamage(damage);
        }
    }
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
        UpdateShieldBar();
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
    public void UpdateShieldBar()
    {
        // Perbarui shield bar berdasarkan nilai currentShield dari karakter
        float shieldFillAmount = (float)character.currentShield / character.maxShield;
        shieldBar.fillAmount = shieldFillAmount;
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