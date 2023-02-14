using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mouse : Enemy
{
    [SerializeField] private float _speed, _amplitude, _creepSpeed, 
        _rotationSpeed, _explodingDuration, _knockbackForce, _knockbackDuration, _explosionRadius;

    private Rigidbody2D _rb;
    private CapsuleCollider2D _collider;
    private Animator _animator;
    private GameObject _player;
    private StatsManager _playerStats;

    private Vector2 _directionToPlayer;

    public bool IsExploding = false, InExplosion = false;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _collider = GetComponent<CapsuleCollider2D>();
        _animator = GetComponent<Animator>();
    }
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _playerStats = _player.GetComponent<StatsManager>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(!Alive) return;
        

        _directionToPlayer = (_player.transform.position - transform.position);

        if(InExplosion) 
        {
            //if(_directionToPlayer.magnitude <= _explosionRadius) Attack();
            return;
        }
        if(IsExploding || IsBlocked) return;

        _directionToPlayer = _directionToPlayer.normalized;

        float angle = Mathf.Atan2(_directionToPlayer.y, _directionToPlayer.x) * Mathf.Rad2Deg;
        angle += 90f;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation =  Quaternion.RotateTowards(transform.rotation, rotation, _rotationSpeed * Time.fixedDeltaTime);

        // float offset = Mathf.Sin(Time.time * _creepSpeed) * _amplitude;
        _rb.velocity = (_directionToPlayer * _speed /*+ Vector2.Perpendicular(_directionToPlayer) * offset*/) * Time.fixedDeltaTime;


    }

    public IEnumerator StartExploding()
    {
        _rb.velocity = Vector2.zero;
        _collider.enabled = false;
        IsExploding = true;
        _animator.SetTrigger("Exploding");
        yield return new WaitForSeconds(_explodingDuration);
        _animator.SetTrigger("Boom");
    }

    public void OnExplosionStart()
    {
        InExplosion = true;
        transform.GetChild(0).gameObject.SetActive(false);
        transform.GetChild(0).gameObject.SetActive(true);
    }

    public void OnExplosionEnd()
    {
       Die();
    }

    public override void Attack()
    {
        _directionToPlayer = (_player.transform.position - transform.position).normalized;
        transform.GetChild(0).gameObject.SetActive(false);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, _directionToPlayer);

        if(hit.collider && hit.collider.gameObject.CompareTag("Keyboard"))
        {
            Debug.Log("Parried");
            return;
        }

        LastAttack = new EnemyAttack(_attackDamage, _directionToPlayer * _knockbackForce, _knockbackDuration);
        _playerStats.TakeDamage(LastAttack);
    }

    public override void TakeDamage(int damage)
    {
        if(IsExploding || InExplosion) return;
        base.TakeDamage(damage);
    }

    public override void Die()
    {
        base.Die();

    }
}
