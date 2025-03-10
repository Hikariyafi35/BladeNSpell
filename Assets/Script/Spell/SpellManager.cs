using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellManager : MonoBehaviour
{
    public GameObject objectToActivate; // GameObject yang akan diaktifkan
    public float activationDuration = 5f; // Durasi aktif dalam detik

    private void Start()
    {
        // Pastikan objectToActivate dimatikan pada awalnya
        objectToActivate.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Cek apakah player yang bersentuhan
        if (other.CompareTag("Player"))
        {
            // Aktifkan GameObject
            objectToActivate.SetActive(true);

            // Mulai durasi untuk mematikan GameObject setelah waktu tertentu
            Invoke("DeactivateObject", activationDuration);
        }
    }

    private void DeactivateObject()
    {
        // Matikan GameObject setelah durasi habis
        objectToActivate.SetActive(false);
    }
}