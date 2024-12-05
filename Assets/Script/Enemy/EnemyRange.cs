using System.Collections;
using UnityEngine;

public class EnemyRange : MonoBehaviour
{
    public float moveSpeed = 2f;
    Transform target;
    Vector2 moveDirection;
    Rigidbody2D rb;
    public float health, maxHealth = 3f;
    private bool facingLeft = true; // Untuk mengecek apakah musuh sedang menghadap kiri
    public int damageCaused;
    public int score;
    private Animator animator;
    public float shootingRange;
    public GameObject bullet;
    public GameObject bulletParent;
    public float fireRate = 1f; // Waktu jeda antara tembakan
    private float nextFireTime = 0f; // Waktu tembakan berikutnya

    // Start is called before the first frame update
    private void Awake() {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Start() {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        health = maxHealth;
    }

    // Update is called once per frame
    void Update() {
    if (target) {
        float distanceFromPlayer = Vector2.Distance(target.position, transform.position);
        moveDirection = (target.position - transform.position).normalized;
        // Jika berada di luar shooting range, musuh bergerak mendekati pemain
        if (distanceFromPlayer > shootingRange) {
            transform.position = Vector2.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
            rb.velocity = new Vector2(moveDirection.x, moveDirection.y) * moveSpeed; // Memastikan musuh terus bergerak
        } else {
            // Jika berada di dalam shooting range, musuh berhenti dan menembak
            rb.velocity = Vector2.zero; // Berhenti bergerak
            Shoot(); 
        }

        // Menjalankan logika flip sprite
        FlipSprite();
        
    }
}

    private void FixedUpdate() {
        // Tidak ada logika tambahan di FixedUpdate, jadi bisa dibiarkan kosong
    }

    // Fungsi untuk menembak peluru
    void Shoot(){
    // Cek apakah sudah waktunya menembak
    if (Time.time > nextFireTime) {
        Instantiate(bullet, bulletParent.transform.position, Quaternion.identity);
        nextFireTime = Time.time + fireRate; // Set waktu untuk tembakan berikutnya
        }
    }

    // Fungsi untuk flip sprite berdasarkan arah gerakan
    void FlipSprite() {
        if (moveDirection.x > 0 && facingLeft) {
            // Jika bergerak ke kanan dan musuh menghadap kiri, flip ke kanan
            Flip();
        } else if (moveDirection.x < 0 && !facingLeft) {
            // Jika bergerak ke kiri dan musuh menghadap kanan, flip ke kiri
            Flip();
        }
    }

    // Fungsi untuk mengubah skala sumbu X untuk flip sprite
    void Flip() {
        Vector3 scale = transform.localScale;
        scale.x *= -1; // Membalikkan skala sumbu X
        transform.localScale = scale;
        facingLeft = !facingLeft; // Membalikkan status apakah menghadap kiri atau tidak
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
        ScoreManager.Instance.AddScore(score);

        StartCoroutine(DestroyAfterAnimation());
    }

    IEnumerator DestroyAfterAnimation() {
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        GetComponent<LootBag>().InstantiateLoot(transform.position);
        Destroy(gameObject);
    }

    // Menggambar gizmo untuk membantu visualisasi jarak tembak
    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.green;
        // Menggambar lingkaran untuk shooting range
        Gizmos.DrawWireSphere(transform.position, shootingRange);
    }
}