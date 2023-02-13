using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crab : Enemy
{
    public CrabState State  {get { return _state; } set { _state = value; NewState(); }}

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

        State = CrabState.Mimic;

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
        if(!Alive || State != CrabState.Chase || IsBlocked) return;

        Vector2 direction = (_player.transform.position - transform.position).normalized;

        _rb.velocity = direction * _speed * Time.fixedDeltaTime;
    }

    void InitialAppear()
    {
        if(State != CrabState.Mimic) return;

        // _trigger.enabled = false;
        State = CrabState.Chase;
        _animator.SetBool("Moving", true);
        
    }
    
    public void StartAttackProcess()
    {
        State = CrabState.Change;
        _rb.velocity = Vector2.zero;
        _animator.SetTrigger("Attack");
    }

    public override void Attack()
    {
        Vector2 direction = (_player.transform.position - transform.position).normalized;
        LastAttack = new EnemyAttack(_attackDamage, direction * _knockbackForce, _knockbackDuration);
        _playerStats.TakeDamage(LastAttack);
    }

    public override void TakeDamage(int damage)
    {
        if(State != CrabState.Idle) return;

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
        if(State == CrabState.Change)
        {
            State = CrabState.Attack;
            //_trigger.enabled = true;
        }
    }

    public void OnAttackEnd()
    {
        if(State == CrabState.Attack)
        {
            //_trigger.enabled = false;
            StartCoroutine(IdleTime());
        }
    }

    IEnumerator IdleTime()
    {
        State = CrabState.Idle;
        yield return new WaitForSeconds(_idleTime);

        if(!Alive) yield break;

        State = CrabState.Change;
        _animator.SetTrigger("Hide");
    }

    public void OnHide()
    {
        State = CrabState.Chase;
        _animator.SetBool("Moving", true);
    }

    private void NewState()
    {
        switch(State)
        {
            case CrabState.Mimic:
                _rb.isKinematic = true;
                break;

            case CrabState.Chase:
                _rb.isKinematic = false;
                break;

            case CrabState.Attack:

                break;

            case CrabState.Idle:
                _rb.isKinematic = false;
                break;

            case CrabState.Change:
                _rb.isKinematic = true;
                break;
        }
    }

}

public enum CrabState
{
    Mimic, Chase, Attack, Idle, Change
}
