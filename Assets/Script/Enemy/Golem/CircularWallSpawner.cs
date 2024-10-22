using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircularWallSpawner : MonoBehaviour
{
    public GameObject wallPrefab; // Prefab untuk dinding berbentuk lingkaran
    public int numberOfWalls = 20; // Jumlah dinding yang akan disusun melingkar
    public float radius = 5f; // Jarak dari pusat ke dinding
    public Transform centerPoint; // Pusat lingkaran (bisa posisi golem)

    void Start()
    {
        SpawnWalls();
    }

    void SpawnWalls()
    {
        float angleStep = 360f / numberOfWalls; // Sudut antara setiap dinding
        float angle = 0f;

        for (int i = 0; i < numberOfWalls; i++)
        {
            // Hitung posisi dinding berdasarkan sudut
            float wallPosX = centerPoint.position.x + Mathf.Sin(angle * Mathf.Deg2Rad) * radius;
            float wallPosY = centerPoint.position.y + Mathf.Cos(angle * Mathf.Deg2Rad) * radius;

            // Buat dinding baru di posisi yang telah dihitung
            Vector2 wallPosition = new Vector2(wallPosX, wallPosY);
            Instantiate(wallPrefab, wallPosition, Quaternion.identity);

            // Tambah sudut untuk dinding berikutnya
            angle += angleStep;
        }
    }
}
