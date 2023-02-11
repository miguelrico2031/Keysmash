using UnityEngine;

public class PlayerMovement2 : MonoBehaviour
{
    public float Speed = 5.0f;
    private Rigidbody2D rb2d;
    private SpriteRenderer spriteRenderer;
    private Vector2 _facingDirection=Vector2.up;
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    void FixedUpdate()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector2 movement = new Vector2(horizontal, vertical);
        if (movement.magnitude > 0)
        {
            _facingDirection = movement.normalized;
        }
        rb2d.velocity = movement * Speed;
        
    }
    public Vector2 GetFacingDirection()
    {
        return _facingDirection;
    }
}
    
