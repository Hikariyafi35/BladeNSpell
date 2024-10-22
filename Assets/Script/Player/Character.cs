using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Character : MonoBehaviour
{
    [SerializeField] public int currentHealth, maxHealth, currentExp, maxExp;
    [SerializeField] private Image expBar;
    [SerializeField] public int currentWave = 1;
    [SerializeField] private TMP_Text waveText;
    [SerializeField] private GamaManager gamaManager; // Referensi ke GameManager

    void Start()
    {
        currentExp = 0; // Mulai dari 0 EXP
        UpdateWaveText();
    }

    void Update()
    {
        // Update EXP bar UI
        expBar.fillAmount = (float)currentExp / maxExp;
    }

    // Fungsi untuk menambah EXP
    public void AddExperience(int amount)
    {
        currentExp += amount;

        // Jika EXP mencapai max, level up atau reset sesuai logika game
        if (currentExp >= maxExp)
        {
            LevelUp();
        }
    }

    void LevelUp()
    {
        currentExp = 0; // Contoh sederhana, reset EXP
        maxExp += 100;   // Tingkatkan batas maxExp, bisa disesuaikan
        currentWave += 1;
        Debug.Log("Wave " + currentWave + " telah dimulai!");
        UpdateWaveText();

        // Cek jika wave adalah kelipatan 10, summon golem
        if (currentWave % 2 == 0)
        {
            gamaManager.SummonGolem();
        }
    }

    void UpdateWaveText(){
        waveText.text = currentWave.ToString();
    }
}