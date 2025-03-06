using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ClaimScore : MonoBehaviour
{
    [SerializeField] private TMP_Text currentScoreText;
    [SerializeField] private TMP_InputField nameInputField;
    [SerializeField] private TMP_Text errorMessageText;
    [SerializeField] private GameObject leaderboardPrefab;

    private void Start()
    {
        // Jika leaderboard belum ada, instantiate prefab Leaderboard
        if (LeaderboardManager2.Instance == null)
        {
            Instantiate(leaderboardPrefab, Vector3.zero, Quaternion.identity);
        }
        // Ambil skor yang disimpan dari PlayerPrefs dan tampilkan
        int score = PlayerPrefs.GetInt("CurrentScore", 0);
        currentScoreText.text =  score.ToString();
        
    }

    public void OnConfirm()
    {
        string playerName = nameInputField.text;

        // Debugging untuk memastikan apakah referensi valid
        if (nameInputField != null)
            Debug.Log("Name input field is valid!");
        else
            Debug.LogError("Name input field is null!");

        if (LeaderboardManager2.Instance != null)
        {
            Debug.Log("LeaderboardManager2 is initialized.");
        }
        else
        {
            Debug.LogError("LeaderboardManager2 is null!");
        }

        if (playerName.Length == 3)
        {
            // Memastikan leaderboard terhubung
            if (LeaderboardManager2.Instance != null)
            {
                SaveToLeaderboard(playerName, PlayerPrefs.GetInt("CurrentScore"));
                errorMessageText.text = ""; // Clear error message
                UnityEngine.SceneManagement.SceneManager.LoadScene("Leaderboard");
            }
            else
            {
                Debug.LogError("LeaderboardManager2.Instance is still null!");
                errorMessageText.text = "Error: Leaderboard not initialized.";
            }
        }
        else
        {
            errorMessageText.text = "Nama harus terdiri dari 3 huruf!";
        }
    }

    private void SaveToLeaderboard(string playerName, int score)
    {
        // Pastikan Instance dari LeaderboardManager2 ada
        LeaderboardManager2.Instance.AddScore(playerName, score);
    }

    public void OnExit()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }
}