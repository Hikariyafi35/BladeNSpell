using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class aimfollow : MonoBehaviour
{
    public Transform player; // drag player ke sini dari Inspector
    public Vector3 offsetFromPlayer = Vector3.zero; // kalau mau geser sedikit posisi weapon
    void Update()
    {
        if (player != null)
        {
            transform.position = player.position + offsetFromPlayer;
        }

    }
}
