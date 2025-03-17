using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldBuff : MonoBehaviour
{
    public int shieldAmount = 5; // Jumlah shield yang diberikan kepada player

    void OnTriggerEnter2D(Collider2D other)
    {
        // Pastikan yang menabrak adalah Player
        if (other.CompareTag("Player"))
        {
            Character character = other.GetComponent<Character>(); // Ambil komponen Character dari player
            if (character != null)
            {
                character.AddShield(shieldAmount);  // Berikan shield ke player
                Destroy(gameObject);  // Hapus shield pickup setelah diberikan
            }
        }
    }
}
