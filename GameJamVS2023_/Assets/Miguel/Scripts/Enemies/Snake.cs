using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : Enemy
{
    [SerializeField] private float _speed, _chargeDuration, _rotationSpeed;
    [SerializeField] private float _knockbackForce;
    [SerializeField] private float _knockbackDuration;

    private Rigidbody2D _rb;
    private Animator _animator;
    private GameObject _player;
    private StatsManager _playerStats;
    private Vector2 _directionToPlayer;
    private bool _charging = true;

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _health = _maxHealth;
    }

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _playerStats = _player.GetComponent<StatsManager>();

        StartCoroutine(ChargeTime());
    }


    void FixedUpdate()
    {
        if(!Alive || IsBlocked) return;
        if(!_charging) _rb.velocity = _directionToPlayer * _speed * Time.fixedDeltaTime;
        else
        {
            _directionToPlayer = (_player.transform.position - transform.position).normalized;
            float angle = Mathf.Atan2(_directionToPlayer.y, _directionToPlayer.x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.AngleAxis(angle + 180f, Vector3.forward);
            transform.rotation =  Quaternion.RotateTowards(transform.rotation, rotation, _rotationSpeed * Time.fixedDeltaTime);
        }
        
    }

    private IEnumerator ChargeTime()
    {
        _charging = true;
        _animator.SetBool("Charging", _charging);
        yield return new WaitForSeconds(_chargeDuration);
        _charging = false;
        _animator.SetBool("Charging", _charging);
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
    }
    public override void TakeDamage(int damage, Vector2 knockbackDirection)
    {
        base.TakeDamage(damage);
    }

    public override void Die()
    {
        base.Die();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(!Alive) return;
        if(other.gameObject.CompareTag("Player")) Attack();
        
        if(other.gameObject.name == "Walls" || other.gameObject.name == "Collideable" || other.gameObject.CompareTag("Player"))
        {
            _rb.velocity = Vector2.zero;
            Debug.Log(other.gameObject.name);
            if(!_charging) StartCoroutine(ChargeTime());
            
        }
    }
}
