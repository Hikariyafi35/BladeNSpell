using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnemyGolem : MonoBehaviour,IBoss
{
    // =============================
    // Boss Name Management
    // =============================
    public string bossName;  // Nama Boss 
    private TMP_Text bossNameText;  // Referensi ke UI teks nama Boss

    // Inisialisasi nama boss pada UI
    public void InitializeBoss(TMP_Text bossNameUI)
    {
        bossNameUI.text = bossName;
        //UpdateBossNameUI();
    }

    // Update UI nama boss di layar
    // private void UpdateBossNameUI()
    // {
    //     if (bossNameText != null)
    //     {
    //         bossNameText.text = bossName;
    //     }
    // }

    // =============================
    // Movement & Combat Variables
    // =============================
    public float moveSpeed = 2f;  // Kecepatan gerakan Golem
    private Transform target;  // Target pemain
    private Rigidbody2D rb;  // Referensi ke Rigidbody2D untuk gerakan fisika
    public float health, maxHealth = 10f;  // Kesehatan Golem
    private bool facingLeft = true;  // Apakah Golem menghadap kiri?
    private Animator animator;  // Animator untuk animasi Golem
    public float shootingRange;  // Jarak tembakan untuk fase 1
    public GameObject bullet;  // Prefab peluru yang ditembakkan
    public Transform bulletParent;  // Tempat spawn peluru
    public float fireRate = 4f;  // Jeda waktu tembakan (fase 1)
    private float nextFireTime = 0f;  // Waktu tembakan berikutnya
    public int bulletCount = 8;  // Jumlah peluru per serangan (fase 1)
    private GamaManager gameManager;  // Referensi ke GameManager

    // =============================
    // Phase 2 Variables
    // =============================
    public float lowHealthFireRate = 0.5f;  // Jeda tembakan fase 2
    public int lowHealthBulletCount = 15;  // Jumlah peluru fase 2
    public float lowHealthShootingRange = 0.5f;  // Jarak tembak fase 2
    private bool isPhaseTwo = false;  // Menandakan apakah Golem sudah masuk fase 2
    private SpriteRenderer spriteRenderer;  // Referensi ke SpriteRenderer untuk mengubah warna

    // =============================
    // Boundary Circle Variables
    // =============================
    public GameObject boundaryCirclePrefab;  // Prefab untuk lingkaran batas
    public float boundaryRadius = 5f;  // Radius lingkaran batas
    public float radiusTriggerWall = 8f;  // Radius trigger untuk spawn dinding
    private bool isWallActive = false;  // Menandakan apakah dinding sudah aktif
    public int numberOfWalls = 20;  // Jumlah dinding yang akan dibuat
    private List<GameObject> boundaryWalls = new List<GameObject>();  // List untuk menyimpan dinding

    // =============================
    // Initialization
    // =============================
    private void Awake()
    {
        // Mendapatkan referensi ke komponen yang dibutuhkan
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        gameManager = FindObjectOfType<GamaManager>();  // Mencari GameManager di scene
    }

    void Start()
    {
        // Menemukan target (pemain)
        target = GameObject.FindGameObjectWithTag("Player").transform;
        health = maxHealth;  // Set HP Golem

        // Menemukan UI untuk nama Boss dan memperbarui tampilan
        bossNameText = GameObject.Find("BossNameText").GetComponent<TMP_Text>();
        //UpdateBossNameUI();
    }

    // =============================
    // Update Loop
    // =============================
    void Update()
    {
        // Jika ada target (pemain)
        if (target)
        {
            float distanceFromPlayer = Vector2.Distance(target.position, transform.position);

            // Fase 2: Golem mengejar pemain dan terus menembak
            if (isPhaseTwo)
            {
                ChasePlayer();
                ShootRadialAttack();
            }
            else
            {
                // Fase 1: Jika pemain terlalu jauh, Golem mengejar
                if (distanceFromPlayer > shootingRange)
                {
                    ChasePlayer();
                }
                // Jika pemain dalam jarak tembak, Golem berhenti dan menembak
                else
                {
                    rb.velocity = Vector2.zero;
                    ShootRadialAttack();
                }
            }

            // Mengubah arah sprite agar menghadap pemain
            FlipSprite();

            // Jika pemain dalam radius untuk memunculkan tembok
            if (distanceFromPlayer <= radiusTriggerWall && !isWallActive)
            {
                isWallActive = true;
                SpawnBoundaryCircle();
            }
        }

        // Masuk ke fase 2 jika HP Golem kurang dari setengah
        if (health <= maxHealth / 2 && !isPhaseTwo)
        {
            EnterPhaseTwo();
        }
    }

    // =============================
    // Movement and Combat
    // =============================
    void ChasePlayer()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        rb.velocity = new Vector2(direction.x, direction.y) * moveSpeed;
    }

    // =============================
    // Radial Attack
    // =============================
    void ShootRadialAttack()
    {
        if (Time.time > nextFireTime)
        {
            float angleStep = 360f / bulletCount;
            float angle = 0f;

            // Tembakkan peluru secara radial
            for (int i = 0; i < bulletCount; i++)
            {
                float bulDirX = bulletParent.position.x + Mathf.Sin((angle * Mathf.PI) / 180f);
                float bulDirY = bulletParent.position.y + Mathf.Cos((angle * Mathf.PI) / 180f);

                Vector3 bulletMoveVector = new Vector3(bulDirX, bulDirY, 0f);
                Vector2 bulletDir = (bulletMoveVector - bulletParent.position).normalized;

                // Membuat dan menembakkan peluru
                GameObject newBullet = Instantiate(bullet, bulletParent.position, Quaternion.identity);
                newBullet.GetComponent<EnemyBulletGolem>().SetMoveDirection(bulletDir);

                angle += angleStep;
            }

            nextFireTime = Time.time + fireRate;
        }
    }

    // =============================
    // Phase Transitions
    // =============================
    void EnterPhaseTwo()
    {
        // Mengubah atribut untuk fase 2
        fireRate = lowHealthFireRate;
        bulletCount = lowHealthBulletCount;
        shootingRange = lowHealthShootingRange;
        isPhaseTwo = true;
        spriteRenderer.color = new Color(0f, 0.925f, 0.910f);  // Mengubah warna Golem
        Debug.Log("Golem entered Phase 2: Faster attack, more bullets, shorter range, and continuous chasing");
    }

    // =============================
    // Sprite Flipping
    // =============================
    void FlipSprite()
    {
        if (target.position.x > transform.position.x && facingLeft)
        {
            Flip();
        }
        else if (target.position.x < transform.position.x && !facingLeft)
        {
            Flip();
        }
    }

    void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;  // Membalikkan skala untuk flip
        transform.localScale = scale;
        facingLeft = !facingLeft;
    }

    // =============================
    // Damage and Death
    // =============================
    public void TakeDamage(float damage)
    {
        health -= damage;
        animator.SetTrigger("Hurt");

        if (health <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        animator.SetTrigger("Death");
        rb.velocity = Vector2.zero;
        moveSpeed = 0;
        GetComponent<Collider2D>().enabled = false;

        StartCoroutine(DestroyAfterAnimation());
        DestroyBoundaryWalls();
    }

    // =============================
    // Boundary Circle and Wall Spawn
    // =============================
    void SpawnBoundaryCircle()
    {
        float angleStep = 360f / numberOfWalls;
        float angle = 0f;

        for (int i = 0; i < numberOfWalls; i++)
        {
            float wallPosX = transform.position.x + Mathf.Sin((angle * Mathf.PI) / 180f) * boundaryRadius;
            float wallPosY = transform.position.y + Mathf.Cos((angle * Mathf.PI) / 180f) * boundaryRadius;

            Vector3 wallPosition = new Vector3(wallPosX, wallPosY, 0f);
            GameObject newWall = Instantiate(boundaryCirclePrefab, wallPosition, Quaternion.identity);
            boundaryWalls.Add(newWall);

            angle += angleStep;
        }
    }

    void DestroyBoundaryWalls()
    {
        foreach (GameObject wall in boundaryWalls)
        {
            Destroy(wall);
        }
        boundaryWalls.Clear();
    }
    IEnumerator DestroyAfterAnimation() {
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        GetComponent<LootBag>().InstantiateLoot(transform.position);
        Destroy(gameObject);

        // Aktifkan kembali exp bar dan matikan health bar saat golem mati
        gameManager.OnBossDeath();
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