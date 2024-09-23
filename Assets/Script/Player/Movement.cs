using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float speed;
    private Vector2 input;
    public Animator anim;
    Rigidbody2D rb;
    private Vector2 lastMoveDirection;
    private bool facingLeft = false; // Set to false if character starts facing right
    public Transform aim;
    bool isWalking = false;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        // Ensure character is initially facing right (adjust as needed)
        if (transform.localScale.x < 0) {
            facingLeft = true;
        }
    }

    void Update() {
        ProcessInput();
        Animate();

        // Flip the character sprite based on the input direction
        if (input.x < 0 && !facingLeft) {
            Flip();
        } else if (input.x > 0 && facingLeft) {
            Flip();
        }

        // Update aim based on movement input
        UpdateAim();
    }

    private void FixedUpdate() {
        // Move the player based on the input vector
        rb.velocity = input * speed;
    }

    void ProcessInput() {
        // Use GetAxisRaw for snappier movement
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        // Set input vector
        input = new Vector2(moveX, moveY);

        // Store the last non-zero input direction
        if (input != Vector2.zero) {
            isWalking = true;
            lastMoveDirection = input;
        } else {
            isWalking = false;
        }

        // Normalize input to prevent faster diagonal movement
        input.Normalize();
    }

    void Animate() {
        // Set animator parameters
        anim.SetFloat("moveX", input.x);
        anim.SetFloat("moveMagnitude", input.magnitude);
        anim.SetFloat("lastMoveX", lastMoveDirection.x);
    }

    void Flip() {
        // Flip the character by inverting the scale
        Vector3 scale = transform.localScale;
        scale.x *= -1; // Flip X-axis
        transform.localScale = scale;

        // Update the facing direction
        facingLeft = !facingLeft;
    }

    void UpdateAim() {
        // Only update aim if the player is walking
        if (isWalking) {
            // Get angle from input direction
            float angle = Mathf.Atan2(input.y, input.x) * Mathf.Rad2Deg;

            // Apply the rotation to the aim, adjusting for facing direction
            if (facingLeft) {
                // Invert the angle if facing left
                aim.rotation = Quaternion.AngleAxis(angle + 180, Vector3.forward);
            } else {
                aim.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            }
        }
    }
}