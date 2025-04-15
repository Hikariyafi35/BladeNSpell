using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] enemyPrefabs;
    [SerializeField] private float baseSpawnRate = 2f; // spawn rate awal (dalam detik)
    [SerializeField] private float minSpawnRate = 0.3f; // batas tercepat
    [SerializeField] private float spawnRadius = 6f;
    [SerializeField] private Transform player;

    private int currentWave = 1;
    private float currentSpawnRate;
    private Coroutine spawnRoutine;

    void Start()
    {
        currentSpawnRate = baseSpawnRate;
        spawnRoutine = StartCoroutine(Spawner());
    }

    void Update()
    {
        // Spawner  follow player
        if (player != null)
        {
            transform.position = player.position;
        }
    }

    public void SetWave(int wave)
    {
        currentWave = wave;
        //spawn lebih cepat berdasarkan wave
        currentSpawnRate = Mathf.Max(minSpawnRate, baseSpawnRate - (wave * 0.1f));

        Debug.Log($"[Spawner] Wave {wave}, spawn rate = {currentSpawnRate}s");

        // Restart coroutine 
        if (spawnRoutine != null)
        {
            StopCoroutine(spawnRoutine);
        }
        spawnRoutine = StartCoroutine(Spawner());
    }

    private IEnumerator Spawner()
    {
        while (true)
        {
            yield return new WaitForSeconds(currentSpawnRate);
            SpawnEnemy();
        }
    }

    private void SpawnEnemy()
    {
        if (enemyPrefabs.Length == 0 || player == null) return;

        Vector2 offset = Random.insideUnitCircle.normalized * spawnRadius;
        Vector3 spawnPos = player.position + new Vector3(offset.x, offset.y, 0f);

        int rand = Random.Range(0, enemyPrefabs.Length);
        GameObject enemyToSpawn = enemyPrefabs[rand];
        Instantiate(enemyToSpawn, spawnPos, Quaternion.identity);
    }
}
