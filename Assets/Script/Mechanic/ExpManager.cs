using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpManager : MonoBehaviour
{
    private bool hasTrigerred;
    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.CompareTag("Player") && !hasTrigerred){
            hasTrigerred = true;
            Destroy(gameObject);
        }
    }
    
}
