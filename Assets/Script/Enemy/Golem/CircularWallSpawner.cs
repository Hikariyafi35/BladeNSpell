using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircularWallSpawner : MonoBehaviour
{
    // =============================
    // Boundary Circle Variables
    // =============================
    public GameObject boundaryCirclePrefab;  // Prefab untuk lingkaran batas
    public float boundaryRadius = 5f;  // Radius lingkaran batas
    public float radiusTriggerWall = 8f;  // Radius trigger untuk spawn dinding
    public int numberOfWalls = 20;  // Jumlah dinding yang akan dibuat
    private List<GameObject> boundaryWalls = new List<GameObject>();  // List untuk menyimpan dinding
    public bool isWallActive = false;  // Menandakan apakah dinding sudah aktif

    // =============================
    // Spawn Boundary Circle
    // =============================
    public void SpawnBoundaryCircle(Vector3 centerPosition)
    {
        if (isWallActive) return;  // Jika tembok sudah aktif, jangan spawn lagi

        float angleStep = 360f / numberOfWalls;
        float angle = 0f;

        for (int i = 0; i < numberOfWalls; i++)
        {
            float wallPosX = centerPosition.x + Mathf.Sin((angle * Mathf.PI) / 180f) * boundaryRadius;
            float wallPosY = centerPosition.y + Mathf.Cos((angle * Mathf.PI) / 180f) * boundaryRadius;

            Vector3 wallPosition = new Vector3(wallPosX, wallPosY, 0f);
            GameObject newWall = Instantiate(boundaryCirclePrefab, wallPosition, Quaternion.identity);

            boundaryWalls.Add(newWall);  // Menambahkan wall ke dalam list

            angle += angleStep;  // Menambah sudut untuk posisi dinding berikutnya
        }

        isWallActive = true;  // Menandakan tembok sudah aktif
    }

    // =============================
    // Destroy Boundary Circle
    // =============================
    public void DestroyBoundaryWalls()
    {
        foreach (GameObject wall in boundaryWalls)
        {
            Destroy(wall);  // Hancurkan setiap dinding
        }
        boundaryWalls.Clear();  // Bersihkan list setelah dinding dihancurkan
        isWallActive = false;  // Reset status dinding
    }
}