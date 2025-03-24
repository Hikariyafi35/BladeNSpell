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
    [SerializeField] private EnemySpawner enemySpawner;
    public int currentShield = 0;
    public int maxShield = 10;
    public Healthbar healthbar;
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
        maxHealth += 10;
        currentExp = 0; // reset EXP
        maxExp += 100;   // Increase maxExp

        currentWave += 1;
        Debug.Log("Wave " + currentWave + " started!");
        UpdateWaveText();
        enemySpawner.SetWave(currentWave);
        // Summon boss every 2 waves
        if (currentWave % 2 == 0)
        {
            int bossIndex = (currentWave / 2) - 1; // Calculate the index of the boss based on the wave
            if (bossIndex < bossPrefabs.Length)
            {
                gamaManager.SummonBoss(bossPrefabs[bossIndex]); // Summon boss from GameManager
            }
        }
    }

    void UpdateWaveText(){
        waveText.text = currentWave.ToString();
    }

    public void AddShield(int shieldAmount)
    {
        currentShield += shieldAmount;
        if (currentShield > maxShield)
            {
                currentShield = maxShield; // Pastikan shield tidak lebih dari maxShield
            }
        Debug.Log("Shield Activated: " + currentShield);
        // Pastikan Healthbar sudah terhubung, dan perbarui shield UI
        if (healthbar != null)
        {
            healthbar.UpdateShieldBar();  // Memperbarui UI shield bar
        }
    }

    public void ReduceShield(int amount)
    {
        currentShield -= amount;
        if (currentShield < 0)
        {
            currentShield = 0; // Jangan biarkan shield menjadi negatif
        }
        Debug.Log("Shield Remaining: " + currentShield);
    }
}