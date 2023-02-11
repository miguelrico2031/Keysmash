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

    void Awake()
    {
        _keyboard = transform.parent.Find("Keyboard").gameObject;
        _rb = GetComponent<Rigidbody2D>();
        _renderer = GetComponent<SpriteRenderer>();
        Direction = Vector2.down;

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
        if(_movement.x != 0 || _movement.y != 0) Direction = moveDirection;
        
        _rb.velocity = moveDirection * _speed;
    }
}
