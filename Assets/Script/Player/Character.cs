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
    [SerializeField] private GameObject[] bossPrefabs; // Daftar prefab boss
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
        // Jika EXP mencapai max, level up atau reset 
        if (currentExp >= maxExp)
        {
            LevelUp();
            AudioManager.Instance.PlaySFX("lvl up");
        }
    }

    void LevelUp()
    {
        maxHealth +=10;
        currentExp = 0; //  reset EXP
        maxExp += 100;   // Tingkatkan batas maxExp
        currentWave += 1;
        Debug.Log("Wave " + currentWave + " telah dimulai!");
        UpdateWaveText();

        // Cek  wave  kelipatan summon boss
        if (currentWave % 2 == 0)
        {
            int bossIndex = (currentWave / 2) - 1; // Hitung index boss berdasarkan wave
        if (bossIndex < bossPrefabs.Length) // Pastikan index tidak keluar dari batas array
            {
                gamaManager.SummonBoss(bossPrefabs[bossIndex]); // Summon boss dari GamaManager
            }
        }
    }

    void UpdateWaveText(){
        waveText.text = currentWave.ToString();
    }
}