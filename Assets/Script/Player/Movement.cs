using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    //I recommend 7 for the move speed, and 1.2 for the force damping
    public float moveSpeed;
    private Vector2 movement;
    public Animator animator;
    Rigidbody2D rb;
    public Transform aim;
    bool isWalking = false;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
    }
    void Update() {
        movement.x = Input.GetAxis("Horizontal");
        movement.y = Input.GetAxis("Vertical");

        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Speed", movement.sqrMagnitude);
    }
    private void FixedUpdate() {
        rb.MovePosition(rb.position + movement * moveSpeed *Time.deltaTime);
        if(isWalking){
            Vector3 vector3 = Vector3.left * movement.x + Vector3.down * movement.y;
            aim.rotation = Quaternion.LookRotation(Vector3.forward, vector3);
        }
    }
}