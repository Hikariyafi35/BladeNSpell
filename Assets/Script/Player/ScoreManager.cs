using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;
    private int totalScore = 0;
    [SerializeField] private TMP_Text scoreText;
    private void Awake() {
        if(Instance == null){
            Instance = this;
        }else{
            Destroy(gameObject);
        }
    }
    public void AddScore(int score){
        totalScore+= score;
        UpdatescoreUI();
    }
    void UpdatescoreUI(){
        if(scoreText!= null){
            scoreText.text = totalScore.ToString();
        }
    }
    public void SaveCurrentScore()
    {
        PlayerPrefs.SetInt("CurrentScore", totalScore); // Menyimpan skor saat ini
        PlayerPrefs.Save();
    }
    public int GetCurrentScore()
    {
        return PlayerPrefs.GetInt("CurrentScore", 0);
    }
}
