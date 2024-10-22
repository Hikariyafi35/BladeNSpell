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
        rb.velocity = input * speed;
        if(isWalking){
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
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;  // Hitung sudut dari input arah
        
        if (facingLeft) {
            // Jika menghadap kiri, kita perlu membalikkan rotasi aim
            angle += 180;  // Tambah 180 derajat saat menghadap kiri
        }

        aim.rotation = Quaternion.Euler(new Vector3(0, 0, angle));  // Rotasi pada sumbu Z saja untuk 2D
    }
}
}