using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class GoblinKing : MonoBehaviour,IBoss
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

    // Variabel tambahan untuk heal dan berhenti serang
    public float healAmount = 2f;  // Jumlah heal setiap 5 detik
    private float stopAttackDuration = 5f;  // Durasi Goblin King berhenti
    private float lastStopAttackTime = 0f;  // Waktu terakhir Goblin King berhenti
    private bool isHealing = false;  // Status apakah Goblin King sedang melakukan healing

    // Variabel lainnya (tidak berubah)
    public GameObject projectilePrefab;
    public Transform shootPoint;
    public float shootDelay = 0.5f;
    public float attackRadius = 5f;
    private bool isPlayerInRange = false;
    public float meleeRadius = 2f;
    public float meleeDelay = 1f;
    private float lastMeleeTime = 0f;
    public float teleportDelay = 2f;
    private float lastTeleportTime = 0f;
    private float lastShootTime = 0f;
    public Collider2D meleeHitbox;
    public Animator animator;

    public void InitializeBoss(TMP_Text bossNameUI)
    {
        bossNameUI.text = bossName;  // Set nama boss di UI
    }
    // Inisialisasi dan Awake tetap tidak berubah
    private void Awake()
    {
        gameManager = FindObjectOfType<GamaManager>();
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        health = maxHealth;

        if (shootPoint == null)
        {
            Debug.LogError("Shoot Point not assigned!");
        }
    }

    // Update method yang dimodifikasi
    private void Update()
    {
        if (player != null)
        {
            // Cek apakah Goblin King harus berhenti serang dan heal
            if (Time.time > lastStopAttackTime + stopAttackDuration)
            {
                // Cek jika health Goblin King berkurang dan belum penuh
                if (health < maxHealth)
                {
                    StartHealing();
                }
                else
                {
                    // Jika health penuh, teruskan aksi normal
                    MoveTowardsPlayer();
                    CheckMeleeAttack(); // Periksa apakah bisa menyerang melee

                    // Periksa apakah pemain berada di dalam area melee
                    if (Vector2.Distance(player.position, transform.position) > meleeRadius)
                    {
                        CheckPlayerInRange();  // Jika tidak dalam area melee, periksa range attack
                    }
                }
            }
            else
            {
                // Lanjutkan dengan serangan seperti biasa jika tidak sedang berhenti
                MoveTowardsPlayer();  // Gerakkan boss menuju pemain
                CheckMeleeAttack();  // Cek apakah bisa menyerang dengan melee

                // Cek jika pemain berada di luar area melee, baru lakukan range attack
                if (Vector2.Distance(player.position, transform.position) > meleeRadius)
                {
                    CheckPlayerInRange();  // Cek apakah player berada di dalam radius attack untuk range
                }

                CheckPlayerOutOfRangeForTeleport();
            }
        }
    }


    // Fungsi untuk memulai healing dan berhenti menyerang
    private void StartHealing()
    {
        isHealing = true;
        lastStopAttackTime = Time.time;  // Reset timer

        // Memicu animasi healing
        animator.SetTrigger("Heal");  // Pastikan trigger "Heal" ada di Animator

        // Hentikan pergerakan Goblin King selama healing
        rb.velocity = Vector2.zero; // Hentikan pergerakan dengan mengatur velocity ke zero

        // Heal Goblin King
        health += healAmount;  // Heal boss
        if (health > maxHealth) health = maxHealth;  // Pastikan health tidak melebihi maxHealth

        // Tampilkan efek healing atau animasi jika ada (optional)
        Debug.Log("Goblin King is healing!");

        // Setelah 1 detik healing selesai dan Goblin King akan kembali menyerang
        Invoke("StopHealing", 1f);  // Goblin King berhenti healing setelah 1 detik
    }

    // Fungsi untuk mengakhiri healing dan melanjutkan aksi normal
    private void StopHealing()
    {
        isHealing = false;
        lastStopAttackTime = Time.time;  // Reset timer untuk healing berikutnya

        // Kembalikan pergerakan Goblin King setelah healing selesai
        // Pastikan jika Goblin King sedang bergerak, velocity kembali ke arah yang benar
        MoveTowardsPlayer();  // Lanjutkan pergerakan menuju pemain setelah selesai healing

        Debug.Log("Goblin King stops healing and is ready to attack!");
    }

    // Fungsi-fungsi lainnya tetap seperti sebelumnya...

    private void CheckPlayerInRange()
    {
        float distanceToPlayer = Vector2.Distance(player.position, transform.position);
        isPlayerInRange = distanceToPlayer <= attackRadius;

        // Pastikan animasi range attack dipicu jika player berada di luar melee radius dan tidak sedang healing
        if (isPlayerInRange && distanceToPlayer > meleeRadius && Time.time > lastShootTime + shootDelay && !isHealing)
        {
            // Memicu animasi range attack sebelum meluncurkan proyektil
            animator.SetTrigger("RangeAttack");

            // Lakukan range attack
            RangeAttack();
        }
    }

    private void CheckPlayerOutOfRangeForTeleport()
    {
        float distanceToPlayer = Vector2.Distance(player.position, transform.position);

        if (distanceToPlayer > attackRadius && Time.time > lastTeleportTime + teleportDelay)
        {
            // Pastikan teleport dipicu hanya setelah delay teleport selesai
            lastTeleportTime = Time.time;  // Reset timer teleport
            TeleportToPlayer();
        }
    }

    public void RangeAttack()
    {
        lastShootTime = Time.time;
        StartCoroutine(ShootProjectiles());
    }

    private IEnumerator ShootProjectiles()
    {
        for (int i = 0; i < 3; i++)
        {
            GameObject projectile = Instantiate(projectilePrefab, shootPoint.position, Quaternion.identity);
            Vector2 direction = (player.position - shootPoint.position).normalized;
            projectile.GetComponent<Rigidbody2D>().velocity = direction * 5f;

            yield return new WaitForSeconds(shootDelay);
        }

        yield return new WaitForSeconds(shootDelay);
    }

    private void CheckMeleeAttack()
    {
        if (Time.time > lastMeleeTime + meleeDelay)
        {
            // Melee attack menggunakan meleeHitbox
            if (meleeHitbox.IsTouching(player.GetComponent<Collider2D>()) && !isHealing)
            {
                // Memicu animasi melee attack
                animator.SetTrigger("MeleeAttack");

                // Menyerang pemain jika terdeteksi dalam hitbox
                MeleeAttack();
            }
        }
    }

    private void MeleeAttack()
    {
        lastMeleeTime = Time.time;
        
        // Menyiapkan array untuk menampung hasil overlap collider
        Collider2D[] hitEnemies = new Collider2D[10]; // Tentukan ukuran array hasil (misalnya 10)
        
        // Menggunakan OverlapCollider untuk mendapatkan colliders yang bertabrakan dengan meleeHitbox
        int numColliders = Physics2D.OverlapCollider(meleeHitbox, new ContactFilter2D().NoFilter(), hitEnemies);

        for (int i = 0; i < numColliders; i++)
        {
            Collider2D enemy = hitEnemies[i];
            if (enemy.CompareTag("Player"))
            {
                // Menyerang pemain jika terdeteksi dalam hitbox
                enemy.GetComponent<Healthbar>().TakeDamage(3);
            }
        }
    }

    private void TeleportToPlayer()
    {
        // Mengatur waktu terakhir teleportasi
        lastTeleportTime = Time.time;

        // Pastikan animasi jump dipicu sebelum teleportasi
        animator.SetTrigger("Jump");
        StartCoroutine(TeleportCoroutine());
    }

    private IEnumerator TeleportCoroutine()
    {
        // Mulai animasi lompat (jump)
        yield return new WaitForSeconds(0.5f);  // Tunggu animasi jump selesai

        // Teleport ke lokasi pemain
        transform.position = player.position;

        // Setelah teleport, animasi jatuh (fall)
        animator.SetTrigger("Fall");

        // Tunggu animasi jatuh selesai (misalnya 0.5 detik)
        yield return new WaitForSeconds(0.5f);

        // Setelah mendarat, Goblin King siap menyerang lagi
        animator.SetTrigger("Idle");  // Kembali ke animasi idle atau serangan lainnya
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
        if (moveDirection.x > 0 && !facingLeft)
        {
            Flip();
        }
        else if (moveDirection.x < 0 && facingLeft)
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
    // Memicu animasi kematian
    animator.SetTrigger("Die");  // Pastikan trigger "Die" ada di Animator

    // Menonaktifkan pergerakan dan menunggu animasi selesai
    rb.velocity = Vector2.zero;  // Menghentikan pergerakan saat kematian
    moveSpeed = 0;  // Memastikan Goblin King tidak bergerak

    // Menonaktifkan collider agar Goblin King tidak berinteraksi dengan dunia setelah mati
    GetComponent<Collider2D>().enabled = false;

    // Menghancurkan GameObject setelah animasi kematian selesai (misalnya setelah 2 detik)
    Destroy(gameObject, 2f);  // Hancurkan setelah 2 detik atau sesuaikan dengan durasi animasi

    // Memanggil fungsi untuk menangani kematian Boss
    gameManager.OnBossDeath();  // Menandakan bahwa Boss telah mati
}

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, meleeRadius);
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