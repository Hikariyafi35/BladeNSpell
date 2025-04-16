using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningOrb : MonoBehaviour
{
    public GameObject buffSpawnerPrefab;  // Prefab BuffSpawner yang akan di-spawn
    private void Start() {
        Destroy(gameObject,6f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Cek apakah yang menabrak adalah Player
        if (other.CompareTag("Player"))
        {
            SpawnBuffSpawner();  // Spawn BuffSpawner di lokasi OrbLightning
            DestroyOrb();         // Hapus orb dari scene setelah tabrakan
        }
    }

    void SpawnBuffSpawner()
    {
        // Spawn BuffSpawner pada posisi yang sama dengan OrbLightning
        Instantiate(buffSpawnerPrefab, transform.position, Quaternion.identity);
    }

    void DestroyOrb()
    {
        // Hapus GameObject OrbLightning dari scene
        Destroy(gameObject);
    }
}
