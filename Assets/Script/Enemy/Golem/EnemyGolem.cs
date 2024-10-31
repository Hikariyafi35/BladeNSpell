using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyGolem : MonoBehaviour
{
    public float moveSpeed = 2f;
    Transform target;
    Rigidbody2D rb;
    public float health, maxHealth = 10f; 
    private bool facingLeft = true;
    public int damageCaused;
    private Animator animator;
    public float shootingRange;
    public GameObject bullet;
    public Transform bulletParent; // Tempat spawn peluru
    public float fireRate = 4f; // Jeda antara serangan (fase 1)
    private float nextFireTime = 0f;
    public int bulletCount = 8; // Jumlah peluru yang ditembakkan dalam satu serangan (fase 1)
    private GamaManager gameManager; // Referensi ke GameManager

    // Variables for phase 2
    public float lowHealthFireRate = 0.5f; // Jeda antara serangan untuk fase 2
    public int lowHealthBulletCount = 15; // Jumlah peluru yang ditembakkan dalam fase 2
    public float lowHealthShootingRange = 0.5f; // Jarak tembak untuk fase 2
    private bool isPhaseTwo = false; // Apakah golem berada di fase 2
    private SpriteRenderer spriteRenderer;

    public GameObject boundaryCirclePrefab; // Prefab untuk lingkaran batas
    public float boundaryRadius = 5f; // Radius lingkaran batas
    public float radiusTriggerWall = 8f; // Radius trigger di mana wall akan muncul
    private bool isWallActive = false; // Apakah tembok sudah aktif
    public int numberOfWalls = 20; // Jumlah dinding yang akan di-spawn
    private List<GameObject> boundaryWalls = new List<GameObject>(); // List untuk menyimpan referensi ke dinding
    

    private void Awake() {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        gameManager = FindObjectOfType<GamaManager>();
    }

    void Start() {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        health = maxHealth;
    }

    void Update() {
        if (target) {
            float distanceFromPlayer = Vector2.Distance(target.position, transform.position);
            
            if (isPhaseTwo) {
                ChasePlayer(); 
                ShootRadialAttack(); 
            } else {
                if (distanceFromPlayer > shootingRange) {
                    ChasePlayer(); 
                } else {
                    rb.velocity = Vector2.zero; 
                    ShootRadialAttack(); 
                }
            }

            FlipSprite(); 

            if (distanceFromPlayer <= radiusTriggerWall && !isWallActive) {
                isWallActive = true; 
                SpawnBoundaryCircle(); 
            }
        }
        
        if (health <= maxHealth / 2 && !isPhaseTwo) {
            EnterPhaseTwo();
        }

    }

    // Fungsi untuk terus mengejar player
    void ChasePlayer() {
    Vector3 direction = (target.position - transform.position).normalized;
    rb.velocity = new Vector2(direction.x, direction.y) * moveSpeed;
}

    void SpawnBoundaryCircle() {
        float angleStep = 360f / numberOfWalls; // Sudut antara setiap dinding berdasarkan jumlah dinding
        float angle = 0f;

        for (int i = 0; i < numberOfWalls; i++) {
            float wallPosX = transform.position.x + Mathf.Sin((angle * Mathf.PI) / 180f) * boundaryRadius;
            float wallPosY = transform.position.y + Mathf.Cos((angle * Mathf.PI) / 180f) * boundaryRadius;

            Vector3 wallPosition = new Vector3(wallPosX, wallPosY, 0f);
            GameObject newWall = Instantiate(boundaryCirclePrefab, wallPosition, Quaternion.identity);

            // Tambahkan dinding ke dalam list untuk dihancurkan nanti
            boundaryWalls.Add(newWall);

            angle += angleStep;
        }
    }

    // radial attack
    void ShootRadialAttack() {
        if (Time.time > nextFireTime) {
            float angleStep = 360f / bulletCount; // Sudut antara setiap peluru
            float angle = 0f;

            for (int i = 0; i < bulletCount; i++) {
                float bulDirX = bulletParent.position.x + Mathf.Sin((angle * Mathf.PI) / 180f);
                float bulDirY = bulletParent.position.y + Mathf.Cos((angle * Mathf.PI) / 180f);

                Vector3 bulletMoveVector = new Vector3(bulDirX, bulDirY, 0f);
                Vector2 bulletDir = (bulletMoveVector - bulletParent.position).normalized;

                // Membuat peluru baru dan mengatur kecepatan
                GameObject newBullet = Instantiate(bullet, bulletParent.position, Quaternion.identity);

                newBullet.GetComponent<EnemyBulletGolem>().SetMoveDirection(bulletDir);

                angle += angleStep; // Tambah sudut untuk peluru berikutnya
            }

            nextFireTime = Time.time + fireRate; // Set waktu tembakan berikutnya
        }
    }

    // Masuk ke Fase 2 ketika HP rendah
    void EnterPhaseTwo() {
        //  atribut serangan untuk fase 2
        fireRate = lowHealthFireRate;
        bulletCount = lowHealthBulletCount;
        shootingRange = lowHealthShootingRange;
        isPhaseTwo = true; //golem sekarang berada di fase 2
        spriteRenderer.color = new Color(0f, 0.925f, 0.910f);
        Debug.Log("Golem has entered phase 2: Faster fire rate, more bullets, shorter shooting range, and continuous chasing");
    }

    void FlipSprite() {
        if (target.position.x > transform.position.x && facingLeft) {
            Flip();
        } else if (target.position.x < transform.position.x && !facingLeft) {
            Flip();
        }
    }

    void Flip() {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
        facingLeft = !facingLeft;
    }

    public void TakeDamage(float damage) {
        health -= damage;
        animator.SetTrigger("Hurt");
        if (health <= 0) {
            Die();
        }
    }
    

    public void Die() {
        animator.SetTrigger("Death");
        rb.velocity = Vector2.zero;
        moveSpeed = 0;
        GetComponent<Collider2D>().enabled = false;

        StartCoroutine(DestroyAfterAnimation());
        DestroyBoundaryWalls();
    }
    

    void DestroyBoundaryWalls() {
        foreach (GameObject wall in boundaryWalls) {
            Destroy(wall);
        }
        boundaryWalls.Clear(); // Bersihkan list setelah dinding dihancurkan
    }

    IEnumerator DestroyAfterAnimation() {
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        GetComponent<LootBag>().InstantiateLoot(transform.position);
        Destroy(gameObject);

        // Aktifkan kembali exp bar dan matikan health bar saat golem mati
        gameManager.OnGolemDeath();
    }

    // Gizmo untuk radius shooting dan radius trigger wall
    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, shootingRange); // Radius shooting
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radiusTriggerWall); // Radius trigger untuk wall
    }
}