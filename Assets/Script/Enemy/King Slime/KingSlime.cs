using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class KingSlime : MonoBehaviour, IBoss
{
    public string bossName;
    private TMP_Text bossNameText;
    private GamaManager gameManager;  // Referensi ke GameManager

    public float moveSpeed = 2f;
    public float health, maxHealth = 10f;
    private bool facingLeft = true;

    private Transform player;
    private Vector2 moveDirection;
    private Rigidbody2D rb;

    // Attack Areas
    public float meleeAttackRadius = 2f;
    public float chargeAttackRadius = 5f;
    public float rangeAttackRadius = 8f;

    // Attack Settings
    public int damageCaused;
    public float chargeSpeed = 10f;
    public float chargeDelay = 1f;
    public GameObject projectilePrefab;
    public Transform projectileSpawnPoint;
    public float rangeAttackCooldown = 2f; // Cooldown untuk range attack

    private bool isCharging = false;
    private bool canShoot = true;
    private LineRenderer lineRenderer;

    public int meleeDamage = 2; // Damage untuk melee attack
    public float meleeAttackCooldown = 1f; // Cooldown untuk melee attack
    private bool canMeleeAttack = true;
    public Animator animator;

    //spawn slime
    public GameObject slimeMinionPrefab; // Prefab slime kecil
    public int slimeCountToSpawn = 5;
    public float spawnRadius = 3f; // Jarak antara King Slime dan pasukannya

    private bool hasSpawnedMinions = false;
    public void InitializeBoss(TMP_Text bossNameUI)
    {
        bossNameUI.text = bossName;
    }
    private void Awake() {
        gameManager = FindObjectOfType<GamaManager>();
    }
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform; // Pastikan Player memiliki tag "Player"
        health = maxHealth;
        lineRenderer = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        if (player != null && !isCharging)
        {
            MoveTowardsPlayer();
            HandleAttacks();
        }
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
        if (moveDirection.x < 0 && facingLeft)
        {
            Flip();
        }
        else if (moveDirection.x > 0 && !facingLeft)
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

    private void HandleAttacks()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= meleeAttackRadius && canMeleeAttack)
        {
            animator.SetTrigger("MeleeAttack");  // Memicu animasi melee attack
            StartCoroutine(MeleeAttack());
        }
        else if (distanceToPlayer <= chargeAttackRadius && !isCharging && distanceToPlayer > meleeAttackRadius)
        {
            animator.SetTrigger("ChargeAttack");  // Bisa menambahkan animasi untuk charge attack, jika ada
            StartCoroutine(ChargeAttack());
        }
        else if (distanceToPlayer <= rangeAttackRadius && canShoot && distanceToPlayer > chargeAttackRadius)
        {
            animator.SetTrigger("RangeAttack");  // Memicu animasi range attack
            StartCoroutine(RangeAttack());
        }
    }


    private IEnumerator MeleeAttack()
    {
        Debug.Log("Performing Melee Attack");
        canMeleeAttack = false;

        // Damage player if within range
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, meleeAttackRadius);
        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag("Player"))
            {
                Healthbar healthbar = hit.GetComponent<Healthbar>();
                if (healthbar != null)
                {
                    healthbar.TakeDamage(meleeDamage);
                }
            }
        }

        // Cooldown before next melee attack
        yield return new WaitForSeconds(meleeAttackCooldown);
        canMeleeAttack = true;
    }

    private IEnumerator ChargeAttack()
    {
        Debug.Log("Preparing Charge Attack");
        isCharging = true;
        rb.velocity = Vector2.zero;

        // Draw line to player position and gradually extend
        Vector3 chargeTarget = player.position;
        Vector3 startPosition = transform.position;
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, startPosition);

        float elapsedTime = 0f;
        while (elapsedTime < chargeDelay)
        {
            elapsedTime += Time.deltaTime;
            Vector3 intermediatePosition = Vector3.Lerp(startPosition, chargeTarget, elapsedTime / chargeDelay);
            lineRenderer.SetPosition(1, intermediatePosition);
            yield return null;
        }

        lineRenderer.SetPosition(1, chargeTarget);
        yield return new WaitForSeconds(chargeDelay);

        lineRenderer.positionCount = 0;

        Vector3 chargeDirection = (chargeTarget - transform.position).normalized;
        rb.velocity = chargeDirection * chargeSpeed;

        yield return new WaitForSeconds(1f); // Charge duration
        rb.velocity = Vector2.zero;
        isCharging = false;
    }

    private IEnumerator RangeAttack()
    {
        Debug.Log("Performing Range Attack");
        canShoot = false; // Mencegah penembakan berulang

        // Spawn projectile
        if (projectilePrefab != null && projectileSpawnPoint != null)
        {
            Vector2 baseDirection = (player.position - transform.position).normalized;

            // Sudut penyebaran
            float angleSpread = 15f; // derajat kanan & kiri
            for (int i = -1; i <= 1; i++)
            {
                // Hitung rotasi berdasarkan sudut
                float angle = angleSpread * i;
                Vector2 spreadDir = Quaternion.Euler(0, 0, angle) * baseDirection;

                GameObject projectile = Instantiate(projectilePrefab, projectileSpawnPoint.position, Quaternion.identity);
                EnemyBulletGolem bulletScript = projectile.GetComponent<EnemyBulletGolem>();
                if (bulletScript != null)
                {
                    bulletScript.SetMoveDirection(spreadDir.normalized);
                }
            }
        }

        // Tunggu cooldown sebelum bisa menembak lagi
        yield return new WaitForSeconds(rangeAttackCooldown);
        canShoot = true;
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
    animator.SetTrigger("Hurt");

    // Cek apakah HP sudah <= 20% dan belum spawn slime
    if (!hasSpawnedMinions && health <= maxHealth * 0.2f)
        {
            hasSpawnedMinions = true;
            SpawnSlimeMinions();
        }

    if (health <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        Debug.Log("Enemy Died");
        animator.SetTrigger("Die");  // Memicu animasi kematian

        rb.velocity = Vector2.zero;
        moveSpeed = 0;
        GetComponent<Collider2D>().enabled = false;
        Destroy(gameObject, 2f);  // Hancurkan objek setelah animasi selesai
        gameManager.OnBossDeath();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, meleeAttackRadius);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, chargeAttackRadius);

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
    private void SpawnSlimeMinions()
    {
        for (int i = 0; i < slimeCountToSpawn; i++)
        {
            // Tentukan posisi acak di sekitar King Slime
            Vector2 randomOffset = Random.insideUnitCircle.normalized * spawnRadius;
            Vector3 spawnPos = transform.position + new Vector3(randomOffset.x, randomOffset.y, 0);

            Instantiate(slimeMinionPrefab, spawnPos, Quaternion.identity);
        }

        Debug.Log("Slime King spawned minions!");
    }
}
