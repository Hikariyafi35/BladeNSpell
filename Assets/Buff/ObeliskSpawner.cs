using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObeliskSpawner : MonoBehaviour
{
    public GameObject buffPrefab;        // GameObject untuk Buff yang akan dipanggil
    public float spawnRadius = 10f;      // Jarak spawn buff di sekitar player
    public float spawnSpeed = 30f;       // Kecepatan rotasi spawn objek mengelilingi player
    public int numberOfBuffs = 5;        // Jumlah buff yang akan di-spawn
    public float buffDuration = 5f;      // Durasi waktu BuffSpawner bertahan sebelum dihancurkan

    private Transform playerTransform;   // Referensi ke transformasi player
    private GameObject[] spawnedBuffs;   // Array untuk menyimpan semua spawned buff
    private float[] angles;              // Array untuk menyimpan sudut awal setiap buff

    void Start()
    {
        // Cari player dengan tag "Player"
        playerTransform = GameObject.FindWithTag("Player").transform;

        // Inisialisasi array untuk menyimpan buff yang di-spawn dan sudut rotasi setiap buff
        spawnedBuffs = new GameObject[numberOfBuffs];
        angles = new float[numberOfBuffs];

        // Spawn buff di sekitar player dalam pola melingkar
        SpawnBuffCircle();

        // Hancurkan BuffSpawner setelah durasi yang ditentukan
        Destroy(gameObject, buffDuration); 
    }

    void SpawnBuffCircle()
    {
        if (playerTransform != null)
        {
            float angleStep = 360f / numberOfBuffs;  // Menghitung langkah sudut untuk penyebaran buff

            for (int i = 0; i < numberOfBuffs; i++)
            {
                float angle = angleStep * i;  // Menghitung sudut untuk setiap buff

                // Menghitung posisi spawn berdasarkan sudut
                Vector2 spawnPosition = (Vector2)playerTransform.position + new Vector2(Mathf.Cos(Mathf.Deg2Rad * angle), Mathf.Sin(Mathf.Deg2Rad * angle)) * spawnRadius;

                // Spawn buff di posisi yang dihitung
                spawnedBuffs[i] = Instantiate(buffPrefab, spawnPosition, Quaternion.identity);

                // Set setiap buff menjadi anak dari Player untuk memudahkan rotasi
                spawnedBuffs[i].transform.parent = playerTransform;

                // Menyimpan sudut awal untuk setiap buff
                angles[i] = angle;
            }
        }
    }

    void Update()
    {
        if (playerTransform != null)
        {
            // Rotasi seluruh buff di sekitar player
            RotateBuffsSmoothly();
        }
    }

    void RotateBuffsSmoothly()
    {
        // Menghitung rotasi per frame
        float rotationAmount = spawnSpeed * Time.deltaTime;

        // Mengupdate posisi setiap buff secara mulus mengelilingi player
        for (int i = 0; i < numberOfBuffs; i++)
        {
            // Update sudut berdasarkan kecepatan rotasi
            angles[i] += rotationAmount;

            // Menghitung posisi baru berdasarkan sudut yang diperbarui
            float angle = angles[i];
            Vector2 newPosition = (Vector2)playerTransform.position + new Vector2(Mathf.Cos(Mathf.Deg2Rad * angle), Mathf.Sin(Mathf.Deg2Rad * angle)) * spawnRadius;

            // Update posisi setiap buff
            spawnedBuffs[i].transform.position = newPosition;
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