using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyBomb : MonoBehaviour
{
    [HideInInspector] public int Damage;
    [HideInInspector] public BombPower BombPower;


    private Rigidbody2D _rb;
    private CircleCollider2D _trigger;
    private BoxCollider2D _collider;
    private Animator _animator;

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _trigger = GetComponent<CircleCollider2D>();
        _trigger.enabled = false;
        _collider = GetComponent<BoxCollider2D>();
        _animator = GetComponent<Animator>();
    }

    public void ActivateBomb(float duration)
    {
        _trigger.enabled = false;
        _collider.enabled = false;
        StartCoroutine(WaitAndActivate(duration));
    }

    private IEnumerator WaitAndActivate(float duration)
    {
        yield return new WaitForSeconds(duration);
        _collider.enabled = true;
    }

    private void Explode()
    {
        _collider.enabled = false;
        _trigger.enabled = true;
        _animator.SetTrigger("Explode");
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.CompareTag("Player")) BombPower.GetBombKey();

        else if(other.gameObject.CompareTag("Enemy")) Explode();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Enemy"))
        {
            Vector2 direction = (other.transform.position - transform.position).normalized;
            other.gameObject.GetComponent<Enemy>().TakeDamage(Damage, direction);
        }
    }

    public void OnExplosionEnd()
    {
        BombPower.GetBombKey();
    }
}
