using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Enemy : MonoBehaviour
{
    
    public bool Alive {get; protected set;} = true;
    public bool IsBlocked = false;
    public EnemyAttack LastAttack;
    
    [SerializeField] protected int _maxHealth;

    [SerializeField] protected int _attackDamage;
    [SerializeField] private GameObject _explosion;

    protected int _health;

    protected DamageAnimation _damageAnimation;
    public UnityEvent<Enemy> OnDie;


    public virtual void TakeDamage(int damage)
    {
        _health -= damage;

        if(_health <= 0) Die();
        else
        {
            if(!_damageAnimation) _damageAnimation = GetComponent<DamageAnimation>();
            GameObject.Find("AudioManager").GetComponent<AudioManager>().PlaySound("EnemyDamage");
            _damageAnimation.StartAnimation();
        }
    }

    public virtual void TakeDamage(int damage, Vector2 knockbackDirection)
    {
        TakeDamage(damage);
    }

    public virtual void Die()
    {
        Alive = false;
        OnDie.Invoke(this);
        if(_explosion) Instantiate(_explosion, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    public abstract void Attack();
}

public struct EnemyAttack
{
    public int damage;
    public Vector2 knockbackForce;
    public float knockbackDuration;

    public EnemyAttack(int damage, Vector2 knockBackForce, float knockbackDuration)
    {
        this.damage = damage;
        this.knockbackForce = knockBackForce;
        this.knockbackDuration = knockbackDuration;
    }

    public EnemyAttack(int damage)
    {
        this.damage = damage;
        this.knockbackForce = Vector2.zero;
        this.knockbackDuration = 0f;
    }
}
