using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class CameraScript : MonoBehaviour
{
    public Transform cameraTransform;
    public float parallaxFactor = 0.5f; // Sesuaikan untuk menentukan seberapa kuat efek parallax

    private Vector3 lastCameraPosition;
    private Tilemap tilemap;

    void Start()
    {
        lastCameraPosition = cameraTransform.position;
        tilemap = GetComponent<Tilemap>();
    }

    void LateUpdate()
    {
        Vector3 deltaMovement = cameraTransform.position - lastCameraPosition;
        transform.position += new Vector3(-deltaMovement.x * parallaxFactor, -deltaMovement.y * parallaxFactor, 0);
        lastCameraPosition = cameraTransform.position;
    }
}
