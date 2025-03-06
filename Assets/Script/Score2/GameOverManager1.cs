// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class GameOverManager1 : MonoBehaviour
// {
//     [SerializeField] private NameInput nameInput; // Referensi ke script NameInput
//     [SerializeField] private LeaderboardManager2 leaderboardManager; // Referensi ke script LeaderboardManager
//     [SerializeField] private ScoreManager scoreManager; // Referensi ke script ScoreManager

//     public void OnClaimScoreButtonPressed()
//     {
//         string playerName = nameInput.GetPlayerName(); // Ambil nama pemain dari input
//         int currentScore = scoreManager.GetCurrentScore(); // Ambil skor saat ini

//         leaderboardManager.AddScoreToLeaderboard(playerName, currentScore); // Tambahkan skor ke leaderboard
//         // Load leaderboard scene
//         UnityEngine.SceneManagement.SceneManager.LoadScene("LeaderboardScene"); // Ganti dengan nama scene leaderboard Anda
//     }
// }