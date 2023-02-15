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

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            _boss.PlayerStats.TakeDamage(_boss.SlashDamage);
        }
    }
}
