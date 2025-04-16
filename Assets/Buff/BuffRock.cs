using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffRock : MonoBehaviour
{
    public GameObject buffPrefab;        // GameObject untuk Buff yang akan dipanggil
    public float spawnRadius = 10f;      // Jarak spawn buff di kiri dan kanan player
    public int numberOfBuffs = 5;        // Jumlah buff yang akan di-spawn di kiri dan kanan player
    public float buffDuration = 5f;      // Durasi waktu BuffSpawner bertahan sebelum dihancurkan

    private Transform playerTransform;   // Referensi ke transformasi player

    void Start()
    {
        // Cari player dengan tag "Player"
        playerTransform = GameObject.FindWithTag("Player").transform;

        // Spawn buff di kiri dan kanan player dalam pola vertikal
        SpawnBuffVertical();

        // Hancurkan BuffSpawner setelah durasi yang ditentukan
        Destroy(gameObject, buffDuration); 
    }

    void SpawnBuffVertical()
    {
        if (playerTransform != null)
        {
            // Menyebarkan buff di kiri dan kanan player secara vertikal
            for (int i = 0; i < numberOfBuffs; i++)
            {
                // Hitung posisi spawn untuk kiri dan kanan player
                float offsetY = i * spawnRadius;  // Jarak vertikal antara setiap buff
                Vector2 leftPosition = (Vector2)playerTransform.position + new Vector2(-spawnRadius, offsetY);
                Vector2 rightPosition = (Vector2)playerTransform.position + new Vector2(spawnRadius, offsetY);

                // Spawn buff di kiri dan kanan player
                Instantiate(buffPrefab, leftPosition, Quaternion.identity);
                Instantiate(buffPrefab, rightPosition, Quaternion.identity);
            }
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
