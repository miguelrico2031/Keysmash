using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [HideInInspector] public bool BlockMovement = false;
    [SerializeField] private float _speed;

    private GameObject _keyboard;
    private Rigidbody2D _rb;
    private SpriteRenderer _renderer;
    private Vector2 _movement;


    public Vector2 Direction {get; private set;}
    public Vector2 RawDirection {get; private set;}

    void Awake()
    {
        _keyboard = transform.parent.Find("Keyboard").gameObject;
        _rb = GetComponent<Rigidbody2D>();
        _renderer = GetComponent<SpriteRenderer>();
        RawDirection = Vector2.down;
        Direction = RawDirection;

    }

    // Update is called once per frame
    void Update()
    {
        _movement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
    }

    void FixedUpdate()  
    {
        if(BlockMovement) return;

        Vector2 moveDirection = _movement.normalized;
        if(_movement.x != 0 || _movement.y != 0)
        {
            Direction = moveDirection;
            RawDirection = _movement;
            
        }
        _rb.velocity = moveDirection * _speed * Time.fixedDeltaTime;
        
        
    }

    public void AddKnockback(Vector2 force, float duration)
    {
        _rb.velocity = Vector2.zero;
        _rb.AddForce(force, ForceMode2D.Impulse);
        StartCoroutine(BlockMovementDuringKnockback(duration));
    }

    private IEnumerator BlockMovementDuringKnockback(float duration)
    {
        if(BlockMovement) yield break;
        BlockMovement = true;
        yield return new WaitForSeconds(duration);
        BlockMovement = false;
    }
}
