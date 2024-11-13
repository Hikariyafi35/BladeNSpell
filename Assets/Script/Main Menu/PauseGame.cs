using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseGame : MonoBehaviour
{
    public GameObject pauseCanvas; // Referensi ke Canvas pause
    private bool isPaused = false;

    void Start()
    {
        // Pastikan Canvas pause tidak aktif di awal
        if (pauseCanvas != null)
        {
            pauseCanvas.SetActive(false);
        }
    }

    void Update()
    {
        // Cek apakah tombol ESC ditekan
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Pause()
    {
        isPaused = true;
        Time.timeScale = 0; // Hentikan waktu dalam game
        if (pauseCanvas != null)
        {
            pauseCanvas.SetActive(true); // Tampilkan Canvas pause
        }
    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1; // Lanjutkan waktu dalam game
        if (pauseCanvas != null)
        {
            pauseCanvas.SetActive(false); // Sembunyikan Canvas pause
        }
    }

    public void QuitGame()
    {
        // Fungsi untuk kembali ke MainMenu scene
        Debug.Log("Returning to MainMenu");

        Time.timeScale = 1; // Pastikan waktu kembali normal sebelum pindah scene
        SceneManager.LoadScene("MainMenu"); // Ganti "MainMenu" dengan nama scene MainMenu Anda
    }
}