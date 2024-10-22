using System.Collections;

using UnityEngine;

public class RandomPosSpawn : MonoBehaviour
{
    public GameObject enemyPrefab;  // Prefab enemy
    public Transform player;        // Referensi ke posisi player
    public float spawnRadius = 10f; // Jarak di sekitar player tempat spawner akan berpindah
    public float spawnInterval = 3f; // Interval waktu untuk spawn enemy
    public float moveInterval = 5f;  // Interval waktu untuk memindahkan spawner
    public float enemyLifetime = 5f; // Waktu hidup enemy sebelum dihancurkan
    public Color gizmoColor = Color.red; // Warna gizmo untuk radius spawn

    private void Start()
    {
        // Mulai coroutine untuk spawn musuh
        StartCoroutine(SpawnEnemy());
        // Mulai coroutine untuk memindahkan spawner
        StartCoroutine(MoveSpawner());
    }

    // Coroutine untuk spawn musuh
    IEnumerator SpawnEnemy()
    {
        while (true)
        {
            // Spawn enemy di posisi spawner saat ini
            GameObject spawnedEnemy = Instantiate(enemyPrefab, transform.position, Quaternion.identity);

            // Hancurkan enemy setelah beberapa detik
            Destroy(spawnedEnemy, enemyLifetime);

            // Tunggu sebelum spawn lagi
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    // Coroutine untuk memindahkan spawner
    IEnumerator MoveSpawner()
    {
        while (true)
        {
            // Hitung posisi baru spawner secara acak di sekitar player dalam radius tertentu
            Vector2 randomDirection = Random.insideUnitCircle.normalized * spawnRadius;
            Vector2 newPosition = (Vector2)player.position + randomDirection;

            // Pindahkan spawner ke posisi baru
            transform.position = newPosition;

            // Tunggu sebelum memindahkan lagi
            yield return new WaitForSeconds(moveInterval);
        }
    }

    // Fungsi untuk menggambar gizmo di Scene view
    private void OnDrawGizmos()
    {
        if (player != null)
        {
            // Set warna gizmo
            Gizmos.color = gizmoColor;
            
            // Gambar lingkaran di sekitar posisi player dengan radius spawnRadius
            Gizmos.DrawWireSphere(player.position, spawnRadius);
        }
    }
}
