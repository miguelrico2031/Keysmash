using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    
    public bool Alive {get; protected set;} = true;
    public bool IsBlocked = false;
    
    [SerializeField] protected int _maxHealth;

    [SerializeField] protected int _attackDamage;

    protected int _health;


    public virtual void TakeDamage(int damage)
    {
        _health -= damage;

        if(_health <= 0) Die();
    }

    public virtual void Die()
    {
        Alive = false;
    }

    public abstract void Attack();
}
