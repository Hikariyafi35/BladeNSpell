using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
    private IBoss currentGolemScript; // Referensi ke script EnemyGolem untuk mengakses health
    public TMP_Text bossNameText; // Referensi ke UI nama boss (drag dari Canvas di Inspector)


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

    // summon golem di posisi GameManager
    public void SummonBoss(GameObject bossPrefab)
    {
        if (currentGolem == null) // Cek jika tidak ada boss lain yang aktif
        {
            if (bossPrefab != null) // Cek jika prefab boss valid
            {
                currentGolem = Instantiate(bossPrefab, transform.position, Quaternion.identity); // Spawn boss di posisi GameManager

                // Cek apakah boss yang disummon mengimplementasikan IBoss
                IBoss bossScript = currentGolem.GetComponent<IBoss>();
                if (bossScript != null)
                {
                    bossScript.InitializeBoss(bossNameText); // Inisialisasi boss dengan nama UI
                    currentGolemScript = bossScript;  // Gunakan IBoss secara langsung
                }

                Debug.Log("Boss summoned: " + currentGolem.name);

                // Tampilkan HP bar boss, sembunyikan EXP bar
                ToggleExpBar(false);
            }
            else
            {
                Debug.LogError("Boss prefab is null!");
            }
        }
    }

// Update Health Bar berdasarkan HP golem atau boss lain
    void UpdateHealthBar()
    {
        if (currentGolemScript != null && healthBar != null)
        {
            // Langsung akses metode IBoss
            float healthPercentage = currentGolemScript.GetCurrentHealth() / currentGolemScript.GetMaxHealth();
            healthBar.fillAmount = healthPercentage; // Update health bar fill amount
        }
    }


    // Fungsi untuk toggle antara EXP Bar dan Golem Health Bar
    public void ToggleExpBar(bool expBarActive)
    {
        expBarCanvas.SetActive(expBarActive); // Aktifkan atau nonaktifkan EXP Bar
        golemHealthBarCanvas.SetActive(!expBarActive); // Kebalikan untuk Health Bar Golem
    }

    // Fungsi yang dipanggil ketika golem mati, untuk mengembalikan ke EXP Bar
    public void OnBossDeath()
    {
        Debug.Log("Golem has died.");
        currentGolem = null; // Reset golem yang aktif
        currentGolemScript = null; // Reset referensi ke script golem
        ToggleExpBar(true); // Aktifkan kembali EXP Bar
    }
}
    // Update Health Bar berdasarkan HP golem