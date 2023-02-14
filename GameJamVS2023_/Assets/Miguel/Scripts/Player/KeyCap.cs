using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyCap : MonoBehaviour
{
    [HideInInspector] public int Damage;
    [HideInInspector] public float Speed;
    [HideInInspector] public float HarmlessSpeed;

    public ProjectilePower ProjectilePower;
    

    private Rigidbody2D _rb;

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    public void Throw(Vector2 direction)
    {
        _rb.AddForce(Speed * direction, ForceMode2D.Impulse);
    }



    private void OnCollisionEnter2D(Collision2D other)
    {
        if(_rb.isKinematic) return;
        if(_rb.velocity.magnitude > HarmlessSpeed)
        {
            if(other.gameObject.CompareTag("Enemy"))
            {
                other.gameObject.GetComponent<Enemy>().TakeDamage(Damage);
                _rb.velocity = Vector2.zero;
            }
        }
        else if (other.gameObject.CompareTag("Player"))
        {
            ProjectilePower.GetKeyCap();
        }
        
        
    }
}
