using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lizard : Enemy
{
    [SerializeField] private float _fireballSpeed, _fireballRotationSpeed, _hideDuration, _hideRadius, _attackSpeed,
        _knockbackForce, _knockbackDuration;
    
    [SerializeField] private FireBall _fireball;

    private Bounds _roomBounds;
    private GameObject _player;
    private SpriteRenderer _spriteRenderer;
    private CircleCollider2D _collider;
    private Rigidbody2D _rb;
    private Animator _animator;
    private Vector2 _directionToPlayer, _lizardDirection;

    private bool _hidden = false;
    private bool atStart = true;

    void Start()
    {
        _health = _maxHealth;
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _collider = GetComponent<CircleCollider2D>();
        _animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();
        _player = GameObject.FindGameObjectWithTag("Player");
        _lizardDirection = Vector2.right;

        var colliders = Physics2D.OverlapCircleAll(transform.position, 0.2f);

        foreach(var col in colliders)
        {
            if(col.CompareTag("Room"))
            {
                Debug.Log("lagartoo bounds");
                _roomBounds = col.bounds;
            }
        }

        StartCoroutine(AttackLoop());
    }

    void FixedUpdate()
    {
        if(!Alive || IsBlocked) return;

        _directionToPlayer = (_player.transform.position - transform.position);

        
        if(_directionToPlayer.magnitude < _hideRadius && !_hidden) StartCoroutine(HideAndAppear());

        _directionToPlayer = _directionToPlayer.normalized;
        
        if(Vector2.Dot(_directionToPlayer, _lizardDirection) < 0)
        {
            _spriteRenderer.flipX = !_spriteRenderer.flipX;

            _lizardDirection *= -1;
        }

    }

    public override void Attack()
    {
        GameObject.Find("AudioManager").GetComponent<AudioManager>().PlaySound("LizardAttack");
        FireBall fireball = Instantiate(_fireball, (Vector2)transform.position + new Vector2(0.5f * _lizardDirection.x, 0.5f),
            Quaternion.AngleAxis(Mathf.Atan2(_directionToPlayer.y, _directionToPlayer.x) * Mathf.Rad2Deg, Vector3.forward));
        fireball.Damage = _attackDamage;
        fireball.Speed = _fireballSpeed;
        fireball.RotationSpeed = _fireballRotationSpeed;
        fireball.KnockbackForce = _knockbackForce;
        fireball.KnockbackDuration = _knockbackDuration;
    }

    IEnumerator HideAndAppear()
    {
        _collider.enabled = false;
        _animator.SetTrigger("Hide");
        _hidden = true;

        yield return new WaitForSeconds(_hideDuration);

        _hidden = false;   
        _spriteRenderer.enabled = true;
        _animator.SetTrigger("Appear");
        AppearAtRandomPoint();
        _collider.enabled = true;
        StartCoroutine(AttackLoop());
    }

    IEnumerator AttackLoop()
    {
        _animator.SetBool("Shoot", false);
        if(atStart)
        {
            atStart = false;
            yield return new WaitForSeconds(Random.Range(0f, 3f));
        }
        
        yield return new WaitForSeconds(1f / _attackSpeed);
        
        if(!_hidden)
        {
            _animator.SetBool("Shoot", true);
            yield return null;
            StartCoroutine(AttackLoop());
        }
    }

    void AppearAtRandomPoint()
    {
        Vector2 newPos = new Vector2(Random.Range(_roomBounds.min.x, _roomBounds.max.x), Random.Range(_roomBounds.min.y, _roomBounds.max.y));
 
        transform.position = newPos;
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
    }

    public override void TakeDamage(int damage, Vector2 knockbackDirection)
    {
        base.TakeDamage(damage);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //if(other.gameObject.CompareTag("Room"))
        //{
        //    Debug.Log("lagartoo bounds");
        //    _roomBounds = other.bounds;
        //}
    }

}
