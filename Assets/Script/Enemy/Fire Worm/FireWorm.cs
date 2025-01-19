using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class FireWorm : MonoBehaviour
{
    //Bosss Name
    public String bossName;
    private TMP_Text bossNameText;
    // Inisialisasi nama boss pada UI
    public void InitializeBoss(TMP_Text bossNameUI)
    {
        bossNameText = bossNameUI;
        UpdateBossNameUI();
    }

    // Update UI nama boss di layar
    private void UpdateBossNameUI()
    {
        if (bossNameText != null)
        {
            bossNameText.text = bossName;
        }
    }
}
