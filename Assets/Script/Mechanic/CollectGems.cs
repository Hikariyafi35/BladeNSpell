using UnityEngine;

public class CollectGems : MonoBehaviour
{
    public Loot gemLoot;

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Player")){
            Character character = other.GetComponent<Character>();
            Healthbar healthbar = other.GetComponent<Healthbar>();
            if(character != null){
                // Tambah EXP dan mainkan SFX untuk 'coin collect' jika expAmount lebih dari 0
                if (gemLoot.expAmount > 0) {
                    character.AddExperience(gemLoot.expAmount);
                    AudioManager.Instance.PlaySFX("coin collect");
                }

                // Tambah Health dan mainkan SFX untuk 'drink potion' jika healthAmount lebih dari 0
                if (gemLoot.healthAmount > 0) {
                    healthbar.AddHealth(gemLoot.healthAmount);
                    AudioManager.Instance.PlaySFX("drink potion");
                }
            }
            Destroy(gameObject);
        }
    }
    
}
