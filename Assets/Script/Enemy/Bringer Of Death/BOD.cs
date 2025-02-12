using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BOD : MonoBehaviour
{
    // Variabel yang sudah ada di script kamu
    public string bossName;
    private TMP_Text bossNameText;
    private GamaManager gameManager;

    public float moveSpeed = 2f;
    public float health, maxHealth = 20f;

    private bool facingLeft = true;
    private Transform player;
    private Vector2 moveDirection;
    private Rigidbody2D rb;

    // Variabel serangan range
    public GameObject projectilePrefab;
    public Transform shootPoint;  // Posisi spawn proyektil (di atas pemain)
    public float rangeAttackDelay = 3f;  // Delay antara setiap serangan range
    private float lastRangeAttackTime = 0f;  // Waktu terakhir range attack dilakukan
    public float rangeAttackRadius = 5f;  // Radius area serangan range (untuk gizmo)

    // Variabel lainnya
    public float meleeRadius = 2f;
    public float meleeDelay = 1f;
    private float lastMeleeTime = 0f;
    public float teleportDelay = 2f;
    private float lastTeleportTime = 0f;

    public void InitializeBoss(TMP_Text bossNameUI)
    {
        bossNameUI.text = bossName;  // Set nama boss di UI
    }

    private void Awake()
    {
        gameManager = FindObjectOfType<GamaManager>();
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        health = maxHealth;
    }

    private void Update()
    {
        if (player != null)
        {
            MoveTowardsPlayer();
            CheckMeleeAttack();
            CheckRangeAttack();
            CheckPlayerOutOfRangeForTeleport();
        }
    }

    // Fungsi untuk memeriksa dan melakukan serangan range
    private void CheckRangeAttack()
    {
        if (Time.time > lastRangeAttackTime + rangeAttackDelay)
        {
            lastRangeAttackTime = Time.time;
            RangeAttack();
        }
    }

    private void RangeAttack()
    {
        // Spawn projectile di atas posisi pemain
        Vector2 spawnPosition = new Vector2(player.position.x, player.position.y + 2f); // 2f offset untuk spawn di atas player
        GameObject projectile = Instantiate(projectilePrefab, spawnPosition, Quaternion.identity);

        // Arahkan proyektil ke pemain
        Vector2 direction = (new Vector2(player.position.x, player.position.y) - spawnPosition).normalized; // Ubah player.position menjadi Vector2
        projectile.GetComponent<Rigidbody2D>().velocity = direction * 5f; // Sesuaikan kecepatan proyektil sesuai keinginan
        

        Debug.Log("Goblin King launches a range attack!");
    }

    private void CheckMeleeAttack()
    {
        if (Time.time > lastMeleeTime + meleeDelay)
        {
            float distanceToPlayer = Vector2.Distance(player.position, transform.position);
            if (distanceToPlayer <= meleeRadius)
            {
                MeleeAttack();
            }
        }
    }

    private void MeleeAttack()
    {
        lastMeleeTime = Time.time;
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, meleeRadius);

        foreach (var enemy in hitEnemies)
        {
            if (enemy.CompareTag("Player"))
            {
                enemy.GetComponent<Healthbar>().TakeDamage(3);
            }
        }
    }

    private void CheckPlayerOutOfRangeForTeleport()
{
    float distanceToPlayer = Vector2.Distance(player.position, transform.position);

    // Memeriksa apakah pemain keluar dari radius range attack
    if (distanceToPlayer > rangeAttackRadius && Time.time > lastTeleportTime + teleportDelay)
    {
        TeleportToPlayer();
    }
}

    private void TeleportToPlayer()
    {
        lastTeleportTime = Time.time;
        transform.position = player.position;
    }

    public void MoveTowardsPlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        moveDirection = direction;
        rb.velocity = new Vector2(moveDirection.x, moveDirection.y) * moveSpeed;
        FlipSprite();
    }

    private void FlipSprite()
    {
        if (moveDirection.x < 0 && !facingLeft)
        {
            Flip();
        }
        else if (moveDirection.x > 0 && facingLeft)
        {
            Flip();
        }
    }

    private void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
        facingLeft = !facingLeft;
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        Debug.Log("Boss Died");
        rb.velocity = Vector2.zero;
        moveSpeed = 0;
        GetComponent<Collider2D>().enabled = false;
        Destroy(gameObject);
        gameManager.OnBossDeath();
    }

    private void OnDrawGizmosSelected()
    {
        // Gambar radius untuk melee attack
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, meleeRadius);

        // Gambar radius untuk range attack
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, rangeAttackRadius);
    }

    public float GetCurrentHealth()
    {
        return health;
    }

    public float GetMaxHealth()
    {
        return maxHealth;
    }
}