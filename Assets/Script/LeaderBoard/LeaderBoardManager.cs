using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[System.Serializable]
public class PlayerScore
{
    public string playerName;
    public int score;

    public PlayerScore(string name, int score)
    {
        playerName = name;
        this.score = score;
    }
}

public class LeaderBoardManager : MonoBehaviour
{
    public TMP_InputField nameInputField;  // Input field untuk nama pemain menggunakan TMP
    public TMP_Text leaderboardText;  // Text untuk menampilkan leaderboard menggunakan TMP_Text
    public Button submitButton;  // Button untuk submit nama
    public Button backButton;  // Button untuk kembali ke menu utama

    private List<PlayerScore> leaderboard = new List<PlayerScore>();  // List leaderboard
    private const int maxLeaderboardEntries = 10;  // Maksimal 10 skor tertinggi

    void Start()
    {
        // Set listener untuk button submit
        submitButton.onClick.AddListener(SubmitScore);
        backButton.onClick.AddListener(BackToMainMenu);

        // Load leaderboard dari PlayerPrefs saat scene dimulai
        LoadLeaderboard();
    }

    // Fungsi untuk menambahkan skor pemain ke leaderboard
    void SubmitScore()
    {
        string playerName = nameInputField.text.ToUpper();  // Ambil nama dari input field dan ubah ke uppercase
        int score = ScoreManager.Instance.GetCurrentScore();  // Ambil skor terakhir yang disimpan

        if (playerName.Length > 0 && playerName.Length <= 3)  // Nama harus 1-3 huruf
        {
            leaderboard.Add(new PlayerScore(playerName, score));  // Menambahkan skor dan nama ke leaderboard
            leaderboard.Sort((x, y) => y.score.CompareTo(x.score));  // Urutkan berdasarkan skor tertinggi

            // Menjaga agar hanya ada 10 skor tertinggi
            if (leaderboard.Count > maxLeaderboardEntries)
            {
                leaderboard.RemoveAt(leaderboard.Count - 1);
            }

            // Simpan leaderboard
            SaveLeaderboard();

            // Tampilkan leaderboard
            DisplayLeaderboard();
        }
        else
        {
            // Feedback jika nama tidak valid (misalnya panjang nama lebih dari 3 karakter)
            Debug.LogWarning("Nama pemain harus 1-3 karakter!");
        }
    }

    // Menyimpan leaderboard ke PlayerPrefs
    void SaveLeaderboard()
    {
        for (int i = 0; i < leaderboard.Count; i++)
        {
            PlayerPrefs.SetString("PlayerName_" + i, leaderboard[i].playerName);
            PlayerPrefs.SetInt("PlayerScore_" + i, leaderboard[i].score);
        }
        PlayerPrefs.Save();
    }

    // Load leaderboard dari PlayerPrefs
    void LoadLeaderboard()
    {
        leaderboard.Clear();
        for (int i = 0; i < maxLeaderboardEntries; i++)
        {
            if (PlayerPrefs.HasKey("PlayerName_" + i))
            {
                string name = PlayerPrefs.GetString("PlayerName_" + i);
                int score = PlayerPrefs.GetInt("PlayerScore_" + i);
                leaderboard.Add(new PlayerScore(name, score));
            }
        }
        leaderboard.Sort((x, y) => y.score.CompareTo(x.score));  // Urutkan berdasarkan skor tertinggi
        DisplayLeaderboard();
    }

    // Menampilkan leaderboard pada UI
    void DisplayLeaderboard()
    {
        leaderboardText.text = "Leaderboard:\n";  // Ganti .text menjadi .text untuk TMP_Text
        foreach (var playerScore in leaderboard)
        {
            leaderboardText.text += playerScore.playerName + ": " + playerScore.score + "\n";
        }
    }

    // Kembali ke menu utama
    void BackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");  // Kembali ke menu utama
    }
}