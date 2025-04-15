using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using TMPro;

public class LevelupManager : MonoBehaviour
{
    public GameObject levelUpCanvas;  // Canvas yang muncul saat level up
    public Button[] skillButtons;  // Tombol-tombol skill
    public TMP_Text[] skillTexts;  // Text untuk menampilkan nama dan deskripsi skill
    private List<Skill> allSkills;  // Daftar seluruh skill yang tersedia
    private List<Skill> playerSkills;  // Daftar skill yang sudah dipilih oleh player

    void Start()
    {
        // Menyiapkan daftar skill yang tersedia
        allSkills = new List<Skill>
        {
            new Skill("Spawn Ball", "Spawn 2 balls around the player."),
            new Skill("Speed Boost", "Increase movement speed for 5 seconds."),
            new Skill("Damage Boost", "Increase damage by 10%."),
            new Skill("Shield", "Gain a temporary shield."),
            new Skill("Teleport", "Teleport to a random location."),
            new Skill("Fireball", "Shoot a fireball at nearby enemies."),
            new Skill("Frost Aura", "Slow down nearby enemies."),
            new Skill("Double Jump", "Gain the ability to double jump."),
            new Skill("Regeneration", "Regenerate health over time.")
        };

        playerSkills = new List<Skill>();  // Daftar skill yang sudah diambil oleh pemain

        // Menyiapkan tombol (belum ada skill yang dipilih pada saat ini)
        foreach (Button button in skillButtons)
        {
            button.onClick.AddListener(() => OnSkillButtonClicked(null));  // Placeholder, sebelum skill terpilih
        }
    }

    // Fungsi untuk memanggil level-up dan menampilkan pilihan skill
    public void ShowLevelUpCanvas()
    {
        levelUpCanvas.SetActive(true);

        // Menampilkan 3 skill yang belum dipilih oleh player
        List<Skill> availableSkills = new List<Skill>();
        foreach (Skill skill in allSkills)
        {
            if (!playerSkills.Contains(skill))
            {
                availableSkills.Add(skill);
            }
        }

        // Pilih 3 skill acak dari yang tersedia
        availableSkills = availableSkills.OrderBy(x => Random.value).Take(3).ToList();

        // Menampilkan skill di tombol
        for (int i = 0; i < 3; i++)
        {
            skillButtons[i].interactable = true;
            skillTexts[i].text = availableSkills[i].skillName + "\n" + availableSkills[i].description;
            skillButtons[i].onClick.RemoveAllListeners();  // Pastikan tidak ada listener sebelumnya
            Skill skill = availableSkills[i];
            skillButtons[i].onClick.AddListener(() => OnSkillButtonClicked(skill));  // Menambahkan listener dengan skill yang dipilih
        }
    }

    // Fungsi saat tombol skill diklik
    void OnSkillButtonClicked(Skill skill)
    {
        if (skill != null && !playerSkills.Contains(skill))
        {
            playerSkills.Add(skill);  // Menambahkan skill yang dipilih ke dalam daftar playerSkills
        }

        if (skill != null)
        {
            skill.UpgradeSkill();  // Upgrade skill yang dipilih
            Debug.Log(skill.skillName + " upgraded to level " + skill.level);
        }

        levelUpCanvas.SetActive(false);  // Menutup canvas level-up
    }
}