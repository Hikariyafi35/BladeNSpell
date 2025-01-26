using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public interface IBoss
{
    void InitializeBoss(TMP_Text bossNameUI); // Untuk inisialisasi nama boss di UI
    void TakeDamage(float damage); // Untuk menangani damage
    void Die(); // Untuk menangani kematian boss
    float GetCurrentHealth(); // Fungsi untuk mendapatkan kesehatan boss
    float GetMaxHealth(); // Fungsi untuk mendapatkan kesehatan maksimal boss
}
