using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Personaje : MonoBehaviour
{
    public GameObject Teclado;
    public Vector2 Direccion;

    public float Velocidad = 3.0f;
    private Rigidbody2D rb2d;
    private SpriteRenderer spriteRenderer;
    private Vector2 _facingDirection;
    void Start()
    {
        _facingDirection = Vector2.down;
        rb2d = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        // MOVIMIENTO
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector2 movement = new Vector2(horizontal, vertical);
        if (movement.magnitude > 0)
        {
            _facingDirection = movement.normalized;
        }
        if (!Dash_Space.EnDash) //!!
        {
            rb2d.velocity = movement * Velocidad;
        }       
    }
    public Vector2 GetFacingDirection()
    {
        return _facingDirection;
    }
    void Update()
    {    
        // HABILIDADES:

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Teclado.GetComponent<Dash_Space>().Usar();
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            Teclado.GetComponent<Melee1_Q>().Usar();
        }
        else if (Input.GetKeyDown(KeyCode.G))
        {
            Teclado.GetComponent<LanzarTecla_G>().Usar();
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            Teclado.GetComponent<Boomerang_R>().Usar();
        }
    }
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Enemigo"))
        {
            Debug.Log("Me Hice Pupa");
        }
       
    }
}
