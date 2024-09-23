using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float speed = 0.5f;
    public Rigidbody2D rb;
    private Vector2 input;
    Animator anim;
    private Vector2 lastMoveDirection;
    private bool facingLeft = true;

    void Start() {
        anim = GetComponent<Animator>();
    }
    void Update(){
        ProcessInput();
        Animate();
        if(input.x < 0 && !facingLeft || input.x > 0 && facingLeft){
        Flip();
    }

    }
    private void FixedUpdate(){
        rb.velocity = input*speed;
    }
    void ProcessInput(){
        float moveX  = Input.GetAxisRaw("Horizontal");
        float moveY  = Input.GetAxisRaw("Vertical");
        if((moveX ==0 && moveY ==0) && (input.x != 0 || input.y != 0)){
            lastMoveDirection = input;
        }
        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");
        
        input.Normalize();
    }
    void Animate(){
        anim.SetFloat("MoveX", input.x);
        anim.SetFloat("MoveY", input.y);
        anim.SetFloat("MoveMagnitude",input.magnitude);
        anim.SetFloat("LastMoveX",lastMoveDirection.x);
        anim.SetFloat("LastMoveY",lastMoveDirection.y);
    }
    void Flip() {
    Vector3 scale = transform.localScale;
    
    if (input.x > 0) {  // Menghadap kanan
        scale.x = -Mathf.Abs(scale.x);  // Pastikan skala positif (menghadap kanan)
        facingLeft = false;  // Sekarang sprite menghadap kanan
    } 
    else if (input.x < 0) {  // Menghadap kiri
        scale.x = Mathf.Abs(scale.x);  // Pastikan skala negatif (menghadap kiri)
        facingLeft = true;  // Sekarang sprite menghadap kiri
    }

    transform.localScale = scale;
}
}