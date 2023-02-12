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

    private bool resting = false;
    private Vector2 _directionToPlayer;

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
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
        _playerStats.TakeDamage(_attackDamage, _directionToPlayer * _knockbackForce, _knockbackDuration);

    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);

        _rb.velocity = Vector2.zero;
        _directionToPlayer = (_player.transform.position - transform.position).normalized;
        _rb.AddForce(-_directionToPlayer * _knockbackForce, ForceMode2D.Impulse);
        StartCoroutine(BlockMovementDuringKnockback(_knockbackDuration));
    }

    private IEnumerator BlockMovementDuringKnockback(float duration)
    {
        if(IsBlocked) yield break;
        IsBlocked = true;
        yield return new WaitForSeconds(duration);
        IsBlocked = false;
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
