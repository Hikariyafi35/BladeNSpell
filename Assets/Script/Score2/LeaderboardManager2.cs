using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LeaderboardManager2 : MonoBehaviour
{
    public static LeaderboardManager2 Instance;

    [SerializeField] private TMP_Text[] nameTexts;
    [SerializeField] private TMP_Text[] scoreTexts;

    private List<LeaderboardEntry> leaderboard = new List<LeaderboardEntry>();

    private void Awake()
    {
        // Inisialisasi Singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);  // Pastikan objek ini tetap ada saat berpindah scene
            Debug.Log("LeaderboardManager2 Instance initialized.");
        }
        else
        {
            Destroy(gameObject);  // Menghindari duplikasi instance
            Debug.Log("Instance already exists, destroying duplicate.");
        }

        LoadLeaderboard();
        UpdateLeaderboardUI();
    }

    public void AddScore(string playerName, int score)
    {
        leaderboard.Add(new LeaderboardEntry(playerName, score));
        leaderboard.Sort((a, b) => b.score.CompareTo(a.score));

        // Batasi leaderboard hanya 5 skor tertinggi
        if (leaderboard.Count > 5)
            leaderboard.RemoveAt(5);

        SaveLeaderboard();
        UpdateLeaderboardUI();
    }

    private void LoadLeaderboard()
    {
        string json = PlayerPrefs.GetString("Leaderboard", "[]");
        leaderboard = JsonUtility.FromJson<LeaderboardList>(json).leaderboard;
    }

    private void SaveLeaderboard()
    {
        string json = JsonUtility.ToJson(new LeaderboardList { leaderboard = leaderboard });
        PlayerPrefs.SetString("Leaderboard", json);
        PlayerPrefs.Save();
    }

    private void UpdateLeaderboardUI()
    {
        for (int i = 0; i < leaderboard.Count; i++)
        {
            nameTexts[i].text = leaderboard[i].name;
            scoreTexts[i].text = leaderboard[i].score.ToString();
        }
    }
}

[System.Serializable]
public class LeaderboardEntry
{
    public string name;
    public int score;

    // Pastikan hanya ada satu konstruktor
    public LeaderboardEntry(string name, int score)
    {
        this.name = name;
        this.score = score;
    }
}

[System.Serializable]
public class LeaderboardList
{
    public List<LeaderboardEntry> leaderboard;
}


