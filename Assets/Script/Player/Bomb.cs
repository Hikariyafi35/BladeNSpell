using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bomb : MonoBehaviour
{
    public LayerMask enemyLayer;  // Layer khusus untuk musuh biasa
     public Image cooldownImage;   // Reference to the Image component
    public float cooldownTime = 5f; // Total cooldown time (in seconds)
    private float currentCooldownTime = 0f; // Current cooldown time remaining
    public AudioClip bombSFX;  // AudioClip untuk SFX ketika bomb diaktifkan
    private AudioSource audioSource;  // AudioSource untuk memutar SFX

    private void Start()
    {
        // Ambil komponen AudioSource
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        // If cooldown is still active, update the cooldown timer
        if (currentCooldownTime > 0)
        {
            currentCooldownTime -= Time.deltaTime;

            // Update the radial fill amount based on the remaining cooldown time
            cooldownImage.fillAmount = 1 - (currentCooldownTime / cooldownTime);
        }
        else
        {
            // If cooldown is over, set the fill amount to 1 (ready to use)
            cooldownImage.fillAmount = 1f;

            // If F key is pressed and the cooldown is over, activate the ultimate ability
            if (Input.GetKeyDown(KeyCode.F))
            {
                ActivateUlt();
                currentCooldownTime = cooldownTime; // Reset the cooldown
            }
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
        // Mainkan SFX ketika bomb diaktifkan
        if (audioSource != null && bombSFX != null)
        {
            audioSource.PlayOneShot(bombSFX); // Memainkan SFX bomb saat ultinya diaktifkan
        }
    }
}