using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class NameInput : MonoBehaviour
{
    [SerializeField] private TMP_InputField nameInputField;

    public string GetPlayerName()
    {
        string playerName = nameInputField.text.ToUpper();
        if (playerName.Length > 3)
        {
            playerName = playerName.Substring(0, 3);
        }
        return playerName;
    }
}
