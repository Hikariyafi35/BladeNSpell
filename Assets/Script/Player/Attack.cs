using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public GameObject melee;
    bool isAttacking = false;
    float attackDuration = 0.3f;
    float atkTimer = 0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckMeleeTimer();
        if(Input.GetMouseButton(0)){
            onAttack();
            
        }
    }

    void onAttack(){
        isAttacking = true;
        
        //call animator
    }
    void CheckMeleeTimer(){
        if(isAttacking){
            atkTimer += Time.deltaTime;
            if(atkTimer >= attackDuration){
                atkTimer = 0;
                isAttacking = false;

            }
        }
    }
}
