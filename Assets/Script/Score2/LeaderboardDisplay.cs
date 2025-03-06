// using System.Collections;
// using System.Collections.Generic;
// using TMPro;
// using UnityEngine;

// public class LeaderboardDisplay : MonoBehaviour
// {
//     [SerializeField] private LeaderboardManager2 leaderboardManager; // Referensi ke LeaderboardManager
//     [SerializeField] private TMP_Text[] playerNames; // Array untuk menyimpan referensi ke teks nama pemain
//     [SerializeField] private TMP_Text[] playerScores; // Array untuk menyimpan referensi ke teks skor pemain

//     private void Start()
//     {
//         DisplayLeaderboard();
//     }

//     private void DisplayLeaderboard()
//     {
//         var leaderboard = leaderboardManager.GetLeaderboard(); // Ambil leaderboard

//         for (int i = 0; i < playerNames.Length; i++)
//         {
//             if (i < leaderboard.Count)
//             {
//                 playerNames[i].text = leaderboard[i].Name; // Set nama pemain
//                 playerScores[i].text = leaderboard[i].Score.ToString(); // Set skor pemain
//             }
//             else
//             {
//                 playerNames[i].text = ""; // Kosongkan jika tidak ada data
//                 playerScores[i].text = ""; // Kosongkan jika tidak ada data
//             }
//         }
//     }
// }
