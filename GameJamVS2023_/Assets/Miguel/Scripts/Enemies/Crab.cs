using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crab : Enemy
{
    [SerializeField] private float _speed, _knockbackDuration, _knockbackForce;
    [SerializeField] private float _initialHiddenTime, _idleTime, _attackSpeed;
    [SerializeField] private CircleCollider2D _trigger;

    private Rigidbody2D _rb;
    private GameObject _player;
    private StatsManager _playerStats;
    private Animator _animator;

    private CrabState _state;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _health = _maxHealth;
    }
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _playerStats = _player.GetComponent<StatsManager>();

        _state = CrabState.Mimic;

        _animator.SetFloat("AttackSpeed", _attackSpeed);

        StartCoroutine(AppearAfterHiddenTime());
    }

    IEnumerator AppearAfterHiddenTime()
    {
        yield return new WaitForSeconds(_initialHiddenTime);
        InitialAppear();
    }

    void FixedUpdate()
    {
        if(_state != CrabState.Chase || IsBlocked) return;

        Vector2 direction = (_player.transform.position - transform.position).normalized;

        _rb.velocity = direction * _speed * Time.fixedDeltaTime;
    }

    void InitialAppear()
    {
        if(_state != CrabState.Mimic) return;

        // _trigger.enabled = false;
        _state = CrabState.Chase;
        _animator.SetBool("Moving", true);
        
    }

    public override void Attack()
    {
        Vector2 direction = (_player.transform.position - transform.position).normalized;
        _playerStats.TakeDamage(_attackDamage, direction * _knockbackForce, _knockbackDuration);
    }

    public override void TakeDamage(int damage)
    {
        if(_state != CrabState.Idle) return;

        base.TakeDamage(damage);

        _rb.velocity = Vector2.zero;
        Vector2 direction = (transform.position - _player.transform.position).normalized;
        _rb.AddForce(direction * _knockbackForce, ForceMode2D.Impulse);
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

    public void OnAttackStart()
    {
        if(_state == CrabState.Change)
        {
            _state = CrabState.Attack;
            //_trigger.enabled = true;
        }
    }

    public void OnAttackEnd()
    {
        if(_state == CrabState.Attack)
        {
            //_trigger.enabled = false;
            StartCoroutine(IdleTime());
        }
    }

    IEnumerator IdleTime()
    {
        _state = CrabState.Idle;
        yield return new WaitForSeconds(_idleTime);
        _state = CrabState.Change;
        _animator.SetTrigger("Hide");
    }

    public void OnHide()
    {
        _state = CrabState.Chase;
        _animator.SetBool("Moving", true);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(!other.gameObject.CompareTag("Player")) return;

        if(_state == CrabState.Mimic || _state == CrabState.Chase)
        {
            _state = CrabState.Change;
            _rb.velocity = Vector2.zero;
            _animator.SetTrigger("Attack");
        }

        else if(_state == CrabState.Attack)
        {
            Attack();
        }

    }

}

public enum CrabState
{
    Mimic, Chase, Attack, Idle, Change
}
