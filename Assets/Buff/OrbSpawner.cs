using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbSpawner : MonoBehaviour
{
    public GameObject[] orbPrefabs;        // Array untuk menyimpan berbagai jenis orb
    public float spawnRadius = 10f;         // Jarak spawn orb di sekitar player
    public float spawnInterval = 3f;        // Interval waktu untuk spawn orb

    private Transform playerTransform;      // Referensi ke transformasi player
    private Vector2 spawnPosition;          // Posisi target untuk spawn (acak di sekitar player)

    void Start()
    {
        // Cari player dengan tag "Player"
        playerTransform = GameObject.FindWithTag("Player").transform;

        // Mulai spawn orb secara berulang dengan interval yang ditentukan
        InvokeRepeating("SpawnOrb", 0f, spawnInterval); 
    }

    void Update()
    {
        // Update posisi spawn orb secara acak di sekitar player
        if (playerTransform != null)
        {
            spawnPosition = (Vector2)playerTransform.position + Random.insideUnitCircle * spawnRadius;
        }
    }

    void SpawnOrb()
    {
        if (playerTransform != null && orbPrefabs.Length > 0)
        {
            // Pilih orb secara acak dari array
            int randomIndex = Random.Range(0, orbPrefabs.Length);

            // Spawn orb yang dipilih secara acak di posisi spawn
            Instantiate(orbPrefabs[randomIndex], spawnPosition, Quaternion.identity);
        }
    }

    // Debug: Untuk menampilkan area spawn di scene view (Gizmo)
    void OnDrawGizmosSelected()
    {
        if (playerTransform != null)
        {
            Gizmos.color = Color.blue;  // Warna Gizmo untuk area spawn orb
            Gizmos.DrawWireSphere(playerTransform.position, spawnRadius);  // Menggambar lingkaran dengan radius spawn
        }
    }
}
