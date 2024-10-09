using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectGems : MonoBehaviour
{
    public Loot gemLoot;

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Player")){
            Character character = other.GetComponent<Character>();
            if(character != null){
                character.AddExperience(gemLoot.expAmount);
            }
            Destroy(gameObject);
        }
    }
    
}
