using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slash : MonoBehaviour
{
    private Boss _boss;
    private Animator _animator;
    
    void Awake()
    {
        _animator = GetComponent<Animator>();
        _boss = GetComponentInParent<Boss>();
    }

    public void SlashAttack()
    {
        _animator.SetTrigger("Slash");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            Vector2 direction = (other.transform.position - transform.position).normalized;
            _boss.PlayerStats.TakeDamage(_boss.SlashDamage, direction * _boss.SlashKnockback, _boss.SlashKnockbackDuration);
        }
    }
}
