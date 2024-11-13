using UnityEngine;

public class Attack : MonoBehaviour
{
    public GameObject melee;
    bool IsAttacking = false;
    float attackDuration = 0.3f;
    float attackTimer = 0f;


    // Update is called once per frame
    void Update()
    {
        ChekMeleeTimer();
        if(Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space)){
            onAttack();
        }
    }
    void onAttack(){
        AudioManager.Instance.PlaySFX("sword swing");
        melee.SetActive(true);
        IsAttacking = true;
    }
    void ChekMeleeTimer(){
        if(IsAttacking){
            attackTimer += Time.deltaTime;
            if(attackTimer >= attackDuration){
                attackTimer = 0;
                IsAttacking = false;
                melee.SetActive(false);
            }
        }
    }
}
