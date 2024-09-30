using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockRotataeHelathbar : MonoBehaviour
{
    public Transform player;  // Referensi ke objek Player
    public Vector3 offset;    // Offset untuk mengatur posisi health bar di bawah player

    void Update()
    {
        // Update posisi health bar untuk mengikuti player
        transform.position = player.position + offset;
    }
}