using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetLeaderboardUI : MonoBehaviour
{
    [SerializeField] private GameObject confirmResetCanvas;

    void Awake()
    {
        if (confirmResetCanvas != null)
            confirmResetCanvas.SetActive(false); // Sembunyikan awal
    }

    // Dipanggil dari tombol trash
    public void ShowConfirmation()
    {
        if (confirmResetCanvas != null)
            confirmResetCanvas.SetActive(true);
    }

    // Dipanggil dari tombol YES
    public void ConfirmReset()
    {
        if (LeaderboardManager2.Instance != null)
            LeaderboardManager2.Instance.ResetLeaderboard();

        if (confirmResetCanvas != null)
            confirmResetCanvas.SetActive(false);
    }

    // Dipanggil dari tombol NO
    public void CancelReset()
    {
        if (confirmResetCanvas != null)
            confirmResetCanvas.SetActive(false);
    }
}

