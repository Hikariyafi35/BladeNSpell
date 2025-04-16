using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffLighting : MonoBehaviour
{
    public float spawnRadius = 5f;  // Jarak spawn di sekitar player
    public float buffDuration = 5f; // Durasi buff aktif
    public Color gizmoColor = Color.yellow; // Warna gizmo untuk jangkauan spawn

    private void Start()
    {
        // Start coroutine untuk spawn buff lightning secara acak di sekitar player
        StartCoroutine(SpawnBuff());
    }

    // Coroutine untuk spawn buff secara acak di sekitar player
    IEnumerator SpawnBuff()
    {
        while (true)
        {
            // Hitung posisi acak di sekitar player dalam radius spawn
            Vector2 randomDirection = Random.insideUnitCircle.normalized * spawnRadius;
            Vector2 newPosition = (Vector2)transform.position + randomDirection;

            // Spawn buff lighting di posisi acak
            GameObject buff = new GameObject("LightningBuff");
            buff.transform.position = newPosition;
            buff.AddComponent<CircleCollider2D>().isTrigger = true; // Tambahkan collider untuk collision detection
            buff.AddComponent<SpriteRenderer>().color = gizmoColor; // Set warna untuk buff, bisa diubah dengan sprite

            // Hancurkan buff setelah beberapa detik
            Destroy(buff, buffDuration);

            // Tunggu sebelum spawn buff lagi
            yield return new WaitForSeconds(buffDuration);
        }
    }

    // Fungsi untuk menggambar gizmo di Scene view
    private void OnDrawGizmos()
    {
        // Set warna gizmo
        Gizmos.color = gizmoColor;

        // Gambar lingkaran yang mewakili area spawn buff di sekitar player
        Gizmos.DrawWireSphere(transform.position, spawnRadius);
    }
}
