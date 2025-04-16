using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thunder : MonoBehaviour
{
    public float damage = 1f;
    public float lightningDuration = 3f;

    // Public string untuk menentukan nama SFX yang akan diputar
    public string sfxName;

    private AudioSource audioSource;

    // AudioClip yang tersedia di dalam script
    public AudioClip thunderSFX;
    public AudioClip fireSFX;
    public AudioClip rockSFX;
    public AudioClip obeliskSFX;

    private void Start() {
        // Memutar SFX saat objek di-spawn
        audioSource = GetComponent<AudioSource>();

        // Memilih AudioClip berdasarkan string yang diberikan
        AudioClip selectedSFX = GetSFXByName(sfxName);

        // Pastikan ada AudioSource dan AudioClip yang valid
        if (audioSource != null && selectedSFX != null) {
            audioSource.PlayOneShot(selectedSFX);  // Memutar SFX sesuai nama yang diberikan
        }

        // Hancurkan objek setelah durasi yang ditentukan
        Destroy(gameObject, lightningDuration);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        // Cek apakah objek tersebut memiliki komponen musuh tertentu
        EnemyMelee enemyMelee = collision.GetComponent<EnemyMelee>();
        EnemyRange enemyRange = collision.GetComponent<EnemyRange>();
        EnemyGolem enemyGolem = collision.GetComponent<EnemyGolem>();
        FireWorm fireWorm = collision.GetComponent<FireWorm>();
        GoblinKing goblinKing = collision.GetComponent<GoblinKing>();

        // Berikan damage ke musuh yang terkena
        if (enemyMelee != null) {
            enemyMelee.TakeDamage(damage);
        }
        if (enemyRange != null) {
            enemyRange.TakeDamage(damage);
        }
        if (enemyGolem != null) {
            enemyGolem.TakeDamage(damage);
        }
        if (fireWorm != null) {
            fireWorm.TakeDamage(damage);
        }
        if (goblinKing != null) {
            goblinKing.TakeDamage(damage);
        }
    }

    // Fungsi untuk memilih AudioClip berdasarkan nama yang diberikan
    AudioClip GetSFXByName(string name) {
        switch (name.ToLower()) {
            case "thunder":
                return thunderSFX;  // Kembalikan SFX Thunder jika nama "thunder" diberikan
            case "fire":
                return fireSFX;  // Kembalikan SFX Fire jika nama "fire" diberikan
            case "rock":
                return rockSFX;  // Kembalikan SFX Rock jika nama "rock" diberikan
            case "obelisk":
                return obeliskSFX;
            default:
                Debug.LogWarning("SFX not found: " + name);
                return null;  // Kembalikan null jika nama tidak valid
        }
    }
}
