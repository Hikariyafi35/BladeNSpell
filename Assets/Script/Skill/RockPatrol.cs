using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockPatrol : Skill
{
    private GameObject rockPrefab;  // Prefab batu yang akan di-spawn
    private float spawnRadius = 3f; // Radius di sekitar pemain untuk spawn batu
    private Transform playerTransform;  // Transform pemain (untuk mendapatkan posisi pemain)

    // Konstruktor untuk skill ini
    public RockPatrol(GameObject rockPrefab, Transform playerTransform) 
        : base("Rock Patrol", "Spawn rocks around the player.")
    {
        this.rockPrefab = rockPrefab;
        this.playerTransform = playerTransform;
    }

    // Fungsi untuk mengaktifkan skill
    public override void ActivateSkill(Character character)
    {
        // Hapus semua batu yang ada sebelumnya
        GameObject[] existingRocks = GameObject.FindGameObjectsWithTag("Rock");
        foreach (var rock in existingRocks)
        {
            GameObject.Destroy(rock);
        }

        // Spawn batu baru sesuai dengan level skill
        int numberOfRocks = 2 + (level - 1) * 2;  // 2 batu di level 1, 4 batu di level 2, 6 batu di level 3

        for (int i = 0; i < numberOfRocks; i++)
        {
            float angle = i * Mathf.PI * 2 / numberOfRocks;  // Menyebar batu secara melingkar
            Vector3 spawnPosition = playerTransform.position + new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * spawnRadius;
            GameObject rock = GameObject.Instantiate(rockPrefab, spawnPosition, Quaternion.identity);
            rock.tag = "Rock";  // Menandai batu dengan tag "Rock"
        }

        Debug.Log($"{skillName} activated: {numberOfRocks} rocks spawned around the player.");
    }
}
