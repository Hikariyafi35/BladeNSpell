using UnityEngine;

public class CollectGems : MonoBehaviour
{
    public Loot gemLoot;

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Player")){
            Character character = other.GetComponent<Character>();
            Healthbar healthbar = other.GetComponent<Healthbar>();
            if(character != null){
                character.AddExperience(gemLoot.expAmount);
                healthbar.AddHealth(gemLoot.healthAmount);
            }
            Destroy(gameObject);
        }
    }
    
}
