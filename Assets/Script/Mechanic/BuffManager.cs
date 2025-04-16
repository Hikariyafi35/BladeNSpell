using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffManager : MonoBehaviour
{
    public GameObject player;  // Referensi ke player
    public GameObject lightningBuffPrefab; // Prefab untuk buff lightning
    public float spawnInterval = 5f; // Interval waktu spawn buff
    public float buffRadius = 3f; // Jarak buff berfungsi

    private void Start()
    {
        // Mulai coroutine untuk spawn buff secara acak
        StartCoroutine(SpawnLightningBuffs());
    }

    // Coroutine untuk spawn buff secara acak
    IEnumerator SpawnLightningBuffs()
    {
        while (true)
        {
            // Hitung posisi acak di sekitar player dalam radius buff
            Vector2 randomDirection = Random.insideUnitCircle.normalized * buffRadius;
            Vector2 spawnPosition = (Vector2)player.transform.position + randomDirection;

            // Spawn buff lighting di posisi acak
            GameObject buff = Instantiate(lightningBuffPrefab, spawnPosition, Quaternion.identity);
            buff.GetComponent<BuffLighting>().spawnRadius = buffRadius; // Set spawn radius untuk gizmo

            // Tunggu sebelum spawn lagi
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    // Fungsi untuk menangani collision antara buff dan player
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Jika objek yang bertabrakan adalah player
        if (other.CompareTag("Player"))
        {
            // Aktifkan buff lightning
            ActivateBuff();
        }
    }

    // Fungsi untuk mengaktifkan buff ketika player bersentuhan
    private void ActivateBuff()
    {
        // Implementasi ketika buff aktif, misalnya memberikan efek ke player
        Debug.Log("Buff Lightning Activated!");
        // Contoh: Tambahkan efek visual atau power-up pada player
    }
}
