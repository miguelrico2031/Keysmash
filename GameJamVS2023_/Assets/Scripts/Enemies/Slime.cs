using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : Enemy
{
    [SerializeField] private float _speed;
    [SerializeField] private float _restDuration, _heapDuration, _knockbackDuration;
    [SerializeField] private float _knockbackForce;

    private Rigidbody2D _rb;
    private GameObject _player;
    private StatsManager _playerStats;
    private Animator _animator;

    private bool resting = false;
    private Vector2 _directionToPlayer;

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _health = _maxHealth;
    }

    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _playerStats = _player.GetComponent<StatsManager>();

        StartCoroutine(HeapRestCycle());
    }


    void FixedUpdate()
    {
        if(!Alive) return;

        if(!resting && !IsBlocked) _rb.velocity = _directionToPlayer * _speed * Time.fixedDeltaTime;
        
        //_animator.SetFloat("Speed", _rb.velocity.sqrMagnitude);
    }

    private IEnumerator HeapRestCycle()
    {
        _directionToPlayer = (_player.transform.position - transform.position).normalized;
        resting = false;
        yield return new WaitForSeconds(_heapDuration);
        _rb.velocity = Vector2.zero;
        resting = true;
        yield return new WaitForSeconds(_restDuration);
        if(Alive) StartCoroutine(HeapRestCycle());
    }

    public override void Attack()
    {
        _directionToPlayer = (_player.transform.position - transform.position).normalized;
        LastAttack = new EnemyAttack(_attackDamage, _directionToPlayer * _knockbackForce, _knockbackDuration);
        _playerStats.TakeDamage(LastAttack);

    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);

        _rb.velocity = Vector2.zero;
        _directionToPlayer = (_player.transform.position - transform.position).normalized;
        _rb.AddForce(-_directionToPlayer * _knockbackForce, ForceMode2D.Impulse);
        StartCoroutine(BlockMovementDuringKnockback(_knockbackDuration));
    }

    public override void TakeDamage(int damage, Vector2 knockbackDirection)
    {
        base.TakeDamage(damage);
        _rb.velocity = Vector2.zero;
        _rb.AddForce(knockbackDirection * _knockbackForce, ForceMode2D.Impulse);
        StartCoroutine(BlockMovementDuringKnockback(_knockbackDuration));
    }

    private IEnumerator BlockMovementDuringKnockback(float duration)
    {
        if(IsBlocked) yield break;
        IsBlocked = true;
        yield return new WaitForSeconds(duration);
        IsBlocked = false;
        _rb.velocity = Vector2.zero;
    }

    public override void Die()
    {
        base.Die();

    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.CompareTag("Player")) Attack();
    }
}
