using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill 
{
    public string skillName;  // Nama skill
    public string description;  // Deskripsi skill
    public int level;  // Level skill (dimulai dari 1)

    // Konstruktor untuk mendefinisikan skill
    public Skill(string skillName, string description)
    {
        this.skillName = skillName;
        this.description = description;
        this.level = 1;  // Semua skill mulai dari level 1
    }

    // Fungsi untuk meningkatkan level skill
    public void UpgradeSkill()
    {
        level++;
        description = $"{skillName} Level {level}";  // Update deskripsi skill saat diupgrade
    }
    public virtual void ActivateSkill(Character character)
    {
        // Implementasi default jika tidak di-override di kelas turunannya
        Debug.Log($"{skillName} activated.");
    }
}
