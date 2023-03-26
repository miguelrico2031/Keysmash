using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeclasAnimacion : MonoBehaviour
{
    private Rigidbody2D _rigidbody2D;
    private void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        ContactPoint2D contact = collision.GetContact(0);
        Vector2 normal = contact.normal;
        Vector2 velocity = _rigidbody2D.velocity;

        velocity = Vector2.Reflect(velocity, normal);
        _rigidbody2D.velocity = velocity;
    }
}
