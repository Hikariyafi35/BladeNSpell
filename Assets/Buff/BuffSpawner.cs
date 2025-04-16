using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffSpawner : MonoBehaviour
{
    public GameObject buffPrefab;        // GameObject untuk Buff yang akan dipanggil
    public float spawnRadius = 10f;      // Jarak spawn buff di sekitar player
    public float spawnInterval = 3f;     // Interval waktu untuk spawn buff
    public float buffDuration = 5f;      // Durasi waktu BuffSpawner bertahan sebelum dihancurkan

    private Transform playerTransform;   // Referensi ke transformasi player
    private Vector2 targetPosition;      // Posisi target untuk spawn (acak di sekitar player)

    void Start()
    {
        // Cari player dengan tag "Player"
        playerTransform = GameObject.FindWithTag("Player").transform;

        // Mulai spawn secara berulang
        InvokeRepeating("SpawnBuff", 0f, spawnInterval);

        // Hancurkan BuffSpawner setelah durasi yang ditentukan
        Destroy(gameObject, buffDuration); 
    }

    void Update()
    {
        // Update posisi spawn buff secara acak di sekitar player
        if (playerTransform != null)
        {
            targetPosition = (Vector2)playerTransform.position + Random.insideUnitCircle * spawnRadius;
        }
    }

    void SpawnBuff()
    {
        if (playerTransform != null)
        {
            // Spawn buff di posisi acak yang ditentukan sekitar player
            Instantiate(buffPrefab, targetPosition, Quaternion.identity);
        }
    }

    // Debug: Untuk menampilkan area spawn di scene view (Gizmo)
    void OnDrawGizmosSelected()
    {
        if (playerTransform != null)
        {
            Gizmos.color = Color.green;  // Warna Gizmo (Area spawn)
            Gizmos.DrawWireSphere(playerTransform.position, spawnRadius);  // Menggambar lingkaran dengan radius spawn
        }
    }
}