using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamaManager : MonoBehaviour
{
    public Transform player;  // Referensi ke transform player
    public float moveSpeed = 5f;  // Kecepatan gerak GameObject yang mengikuti
    public float followHeightOffset = 2f;  // Offset tinggi dari player agar selalu berada di atasnya
    public GameObject golemPrefab;  // Prefab untuk golem yang akan di-summon
    public GameObject expBarCanvas; // Referensi ke canvas EXP Bar
    public GameObject golemHealthBarCanvas; // Referensi ke canvas HP Bar Golem
    public Image healthBar;  // Referensi untuk Health Bar UI

    private GameObject currentGolem; // Referensi ke golem yang disummon
    private EnemyGolem currentGolemScript; // Referensi ke script EnemyGolem untuk mengakses health

    void Update()
    {
        // Tentukan posisi target dengan offset di atas player
        Vector3 targetPosition = new Vector3(player.position.x, player.position.y + followHeightOffset, player.position.z);

        // Pindahkan GameManager ke posisi target secara bertahap dengan kecepatan tertentu
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        // Update health bar golem jika ada golem yang disummon
        if (currentGolem != null && currentGolemScript != null)
        {
            UpdateHealthBar();
        }
    }

    // Fungsi untuk mensummon golem di posisi GameManager
    public void SummonGolem()
    {
        if (currentGolem == null) // Cek jika golem belum ada
        {
            currentGolem = Instantiate(golemPrefab, transform.position, Quaternion.identity); // Spawn golem di posisi GameManager
            currentGolemScript = currentGolem.GetComponent<EnemyGolem>(); // Simpan referensi ke script EnemyGolem
            Debug.Log("Golem summoned at position: " + transform.position);

            // Tampilkan HP bar golem, sembunyikan EXP bar
            ToggleExpBar(false);
        }
    }

    // Fungsi untuk toggle antara EXP Bar dan Golem Health Bar
    public void ToggleExpBar(bool expBarActive)
    {
        expBarCanvas.SetActive(expBarActive); // Aktifkan atau nonaktifkan EXP Bar
        golemHealthBarCanvas.SetActive(!expBarActive); // Kebalikan untuk Health Bar Golem
    }

    // Fungsi yang dipanggil ketika golem mati, untuk mengembalikan ke EXP Bar
    public void OnGolemDeath()
    {
        Debug.Log("Golem has died.");
        currentGolem = null; // Reset golem yang aktif
        currentGolemScript = null; // Reset referensi ke script golem
        ToggleExpBar(true); // Aktifkan kembali EXP Bar
    }

    // Update Health Bar berdasarkan HP golem
    void UpdateHealthBar()
    {
        if (currentGolemScript != null && healthBar != null)
        {
            float healthPercentage = currentGolemScript.health / currentGolemScript.maxHealth;
            healthBar.fillAmount = healthPercentage; // Update health bar fill amount
        }
    }
}