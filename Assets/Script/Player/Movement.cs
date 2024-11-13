using UnityEngine;

public class Movement : MonoBehaviour
{
    public float speed = 0.5f;
    public Rigidbody2D rb;
    private Vector2 input;
    Animator anim;
    public Vector2 lastMoveDirection;
    private bool facingLeft = true;
    public Transform aim;
    public bool isWalking = false;
    public bool isDead = false; 
    public bool isGameOver = false;

    void Start() {
        anim = GetComponent<Animator>();
    }
    void Update(){
        if (isDead || isGameOver) {
            input = Vector2.zero; // Set input ke nol jika karakter mati atau game over
        } else {
            ProcessInput();
        }
        Animate();

        if (input.x < 0 && !facingLeft || input.x > 0 && facingLeft){
            Flip();
        }
    }

    private void FixedUpdate(){
        if (!isDead && !isGameOver) { // Cek kondisi sebelum menggerakkan karakter
            rb.velocity = input * speed;
        } else {
            rb.velocity = Vector2.zero; // Set ke nol agar karakter benar-benar berhenti
        }

        if(isWalking && !isDead && !isGameOver) {
            Vector2 direction = new Vector2(input.x, input.y);
            UpdateAimDirection(direction);
        }
    }

    void ProcessInput(){
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        if(moveX == 0 && moveY == 0 && (input.x != 0 || input.y != 0)){
            isWalking = false;
            lastMoveDirection = input;
            UpdateAimDirection(lastMoveDirection);
        }else if(moveX != 0 || moveY != 0){
            isWalking = true;
        }

        input.x = moveX;
        input.y = moveY;

        input.Normalize();
    }

    void Animate(){
        anim.SetFloat("MoveX", input.x);
        anim.SetFloat("MoveY", input.y);
        anim.SetFloat("MoveMagnitude", input.magnitude);
        anim.SetFloat("LastMoveX", lastMoveDirection.x);
        anim.SetFloat("LastMoveY", lastMoveDirection.y);
    }

    void Flip() {
        Vector3 scale = transform.localScale;

        if (input.x > 0) {
            scale.x = -Mathf.Abs(scale.x);
            facingLeft = false;
        } else if (input.x < 0) {
            scale.x = Mathf.Abs(scale.x);
            facingLeft = true;
        }

        transform.localScale = scale;
    }

    void UpdateAimDirection(Vector2 direction) {
        if (direction != Vector2.zero) {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            if (facingLeft) {
                angle += 180;
            }

            aim.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }
    }

    // Fungsi untuk mengaktifkan kondisi mati
    public void SetDead(bool value) {
        isDead = value;
    }

    // Fungsi untuk mengaktifkan kondisi game over
    public void SetGameOver(bool value) {
        isGameOver = value;
    }
}