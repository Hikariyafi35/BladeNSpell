using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public LayerMask enemyLayer;  // Layer khusus untuk musuh biasa

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F)) // Ketika tombol F ditekan
        {
            ActivateUlt();
        }
    }

    // Fungsi untuk mengaktifkan ulti
    void ActivateUlt()
    {
        // Menemukan semua objek dalam radius tertentu (misalnya, seluruh scene atau area tertentu)
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        // Menghancurkan setiap musuh yang ditemukan
        foreach (GameObject enemy in enemies)
        {
            // Periksa apakah musuh memiliki layer "Enemy" (bukan Boss)
            if (((1 << enemy.layer) & enemyLayer) != 0)
            {
                // Jika ya, panggil fungsi Die() untuk menghapus musuh
                EnemyMelee enemyMelee = enemy.GetComponent<EnemyMelee>();
                if (enemyMelee != null)
                {
                    enemyMelee.Die(); // Panggil fungsi Die() untuk menghapus musuh
                }

                EnemyRange enemyRange = enemy.GetComponent<EnemyRange>();
                if (enemyRange != null)
                {
                    enemyRange.Die(); // Panggil fungsi Die() untuk menghapus musuh
                }
            }
        }
    }
}
