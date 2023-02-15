using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thrust : MonoBehaviour
{
    private Boss _boss;
    private Animator _animator;
    void Awake()
    {
        _animator = GetComponent<Animator>();
        _boss = GetComponentInParent<Boss>();
    }
    
    public void ThrustAttack()
    {
        _animator.SetTrigger("Thrust");
    }

    public void EndThrustAttack()
    {
        _animator.SetTrigger("Fold");
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            Vector2 direction = (other.transform.position - transform.position).normalized;
            _boss.PlayerStats.TakeDamage(_boss.ThrustDamage, direction * _boss.ThrustKnockback, _boss.ThrustKnockbackDuration);
        } 
    }
}
