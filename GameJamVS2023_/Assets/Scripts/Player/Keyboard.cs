using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Keyboard : MonoBehaviour
{
    public static Keyboard Instance;

    public KeyboardAttack AttackMode = KeyboardAttack.None;

    [SerializeField] private Transform _rightHand, _leftHand, _endAttackTransform;
    [SerializeField] private float _followHandOffset;

    private BoxCollider2D _collider;
    private Rigidbody2D _rb;
    private Animator _animator;
    private HandsFollowPlayer _rightHandFollowPlayer, _leftHandFollowPlayer;
    private RotateHands _rotateHands;
    private PlayerMovement _playerMovement;
    private StatsManager _playerStats;

    private float _meleeAttackTime, _meleeAttackDuration, _meleeAcceleration;
    private int _meleeAttackDamage;


    private Vector2 _meleeStartPos, _meleeMiddlePos, _meleeEndPos;

    private bool _boomerangReturning = false;
    private Vector2 _boomerangDirection;
    private float _boomerangAcceleration, _boomerangSpeed, _boomerangAttackDuration, _boomerangAttackTime, _boomerangRotationSpeed;
    private int _boomerangAttackDamage;

    private float _shieldBashDuration, _shieldBashTime;
    private Vector2 _shieldStartPos, _shieldEndPos, _shieldStartFw, _shieldEndFw;

    private void Awake()
    {
        Instance = this;
        _collider = GetComponent<BoxCollider2D>();
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _rightHandFollowPlayer = _rightHand.GetComponent<HandsFollowPlayer>();
        _leftHandFollowPlayer = _leftHand.GetComponent<HandsFollowPlayer>();

    }
    private void Start()
    {
        _playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        _playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<StatsManager>();
        _rotateHands = _playerMovement.GetComponentInChildren<RotateHands>();
        _collider.enabled = false;
    }

    
    void FixedUpdate()
    {

        switch(AttackMode)
        {
        case KeyboardAttack.Melee:
            transform.SetPositionAndRotation(_rightHand.position + (Vector3)_playerMovement.Direction * _followHandOffset, _rightHand.rotation);
            MeleeAttack();
            break;
        
        case KeyboardAttack.Boomerang:
            BoomerangAttack();
            break;

        case KeyboardAttack.Shield:
            transform.position = (_rightHand.position + (Vector3)_playerMovement.Direction * _followHandOffset);
            ShieldCover();
            break;

        case KeyboardAttack.None:
            transform.SetPositionAndRotation(_rightHand.position + (Vector3)_playerMovement.Direction * _followHandOffset, _rightHand.rotation);
            break;
        }

        

    }

    public void StartMeleeAttack(float attackDuration, float acceleration, int attackDamage)
    {
        //Attacking = true;
        AttackMode = KeyboardAttack.Melee;

        _meleeAttackDuration = attackDuration;
        _meleeAttackTime = 0f;
        _meleeAttackDamage = attackDamage;
        _meleeAcceleration = acceleration;


        _rightHandFollowPlayer.SetHandInPosition();
        _rightHandFollowPlayer.IsBlocked = true;

        _leftHandFollowPlayer.SetHandInPosition();
        _leftHandFollowPlayer.IsBlocked = true;
        //block rotatehands
        //_rotateHands.SetHandsInRotation();
        //_rotateHands.IsBlocked = true;
        _playerMovement.BlockMovement = true;
        //_playerMovement.GetComponent<Rigidbody2D>().velocity = Vector2.zero;

        _collider.enabled = true;

        //Calculate Melee attack points
        _meleeStartPos = _rightHand.position;
        _meleeEndPos = _endAttackTransform.position;
        _meleeMiddlePos = _playerMovement.transform.position + (Vector3) _playerMovement.Direction * 2f;
    }

    private void MeleeAttack()
    {
        _meleeMiddlePos = _meleeMiddlePos = _playerMovement.transform.position + (Vector3) _playerMovement.Direction * 2f;


        _rightHand.position = Bezier.GetBezier3(_meleeAttackTime, _meleeAttackDuration, _meleeStartPos, _meleeMiddlePos, _meleeEndPos);

        _meleeAttackTime += Time.fixedDeltaTime * _meleeAcceleration;
        if(_meleeAttackTime >= _meleeAttackDuration) EndMeleeAttack();
    }

    private void EndMeleeAttack()
    {
        //Attacking = false;
        AttackMode = KeyboardAttack.None;
        _rightHandFollowPlayer.IsBlocked = false;
        _leftHandFollowPlayer.IsBlocked = false;
        //unblock rotatehands?
        _playerMovement.BlockMovement = false;
        _collider.enabled = false;
    }

    public void StartBoomerangAttack(float attackDuration, float speed, float rotationSpeed, int attackDamage)
    {
        AttackMode = KeyboardAttack.Boomerang;

        _boomerangAttackDuration = attackDuration;
        _boomerangAttackDamage = attackDamage;
        _boomerangSpeed = speed;
        _boomerangRotationSpeed = rotationSpeed;

        _boomerangDirection = _playerMovement.Direction;
        _boomerangAttackTime = 0f;

        _boomerangReturning = false;
        _collider.enabled = true;
        
    }

    private void BoomerangAttack()
    {
        
        if(_boomerangReturning)
        {
            Vector2 distanceToHand = _rightHand.position - transform.position;
            _boomerangDirection = distanceToHand.normalized;

            if(distanceToHand.magnitude <= 0.1f)
            {
                EndBoomerangAttack();
            }
        }
        else
        { 
            _boomerangAttackTime += Time.fixedDeltaTime;

            if(_boomerangAttackTime >= _boomerangAttackDuration)
            {
                _boomerangReturning = true;
                //_boomerangAcceleration *= -1f;
                _rb.velocity = Vector2.zero;
            }
        }
        _rb.velocity = _boomerangDirection * _boomerangSpeed * Time.fixedDeltaTime;
        _rb.MoveRotation(_rb.rotation + _boomerangRotationSpeed * Time.fixedDeltaTime);
    }

    private void EndBoomerangAttack()
    {
        AttackMode = KeyboardAttack.None;
        _collider.enabled = false;
    }

    public void StartShieldCover(float bashDuration)
    {

        _shieldBashDuration = bashDuration;

        AttackMode = KeyboardAttack.Shield;

        _rightHandFollowPlayer.SetHandInPosition();
        _rightHandFollowPlayer.IsBlocked = true;

        _leftHandFollowPlayer.SetHandInPosition();
        _leftHandFollowPlayer.IsBlocked = true;
        //block rotatehands
        _rotateHands.SetHandsInRotation();
        _rotateHands.IsBlocked = true;

        _playerMovement.BlockMovement = true;
        _playerMovement.GetComponent<Rigidbody2D>().velocity = Vector2.zero;

        _collider.enabled = true;
        _collider.isTrigger = false;
        _rb.freezeRotation = true;

        _shieldStartPos = _rightHand.position;
        _shieldStartFw = _playerMovement.Direction;
        _shieldEndPos = _playerMovement.transform.position + (Vector3) _playerMovement.Direction * 0.7f;
        _shieldEndFw = _playerMovement.Direction;

        _shieldBashTime = 0f;

        _animator.SetTrigger("Shield");
        transform.Rotate(Vector3.forward, 90f);
    }

    private void ShieldCover()
    {
        _shieldBashTime += Time.fixedDeltaTime;
        if(_shieldBashTime >= _shieldBashDuration) return;

        _shieldEndPos = _playerMovement.transform.position + (Vector3) _playerMovement.Direction * 0.7f;
        _shieldEndFw = _playerMovement.Direction;

        _rightHand.position = Bezier.GetBezier2(_shieldBashTime, _shieldBashDuration, _shieldStartPos, _shieldStartFw, _shieldEndPos, _shieldEndFw);
        
        transform.rotation = _rightHand.rotation;
        transform.Rotate(Vector3.forward, 90f);
        
    }

    public void EndShieldCover()
    {
        AttackMode = KeyboardAttack.None;
        _rightHandFollowPlayer.IsBlocked = false;
        _leftHandFollowPlayer.IsBlocked = false;
        //unblock rotatehands
        _rotateHands.IsBlocked = false;
        _playerMovement.BlockMovement = false;
        
        _rb.freezeRotation = false;
        _collider.isTrigger = true;
        _collider.enabled = false;
        
        _animator.SetTrigger("EndShield");
        transform.Rotate(Vector3.forward, -90f);
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Enemy"))
        {
            var enemy = other.gameObject.GetComponent<Enemy>();
            switch(AttackMode)
            {
            case KeyboardAttack.Melee:
                enemy.TakeDamage(_meleeAttackDamage);
                break;
            
            case KeyboardAttack.Boomerang:
                enemy.TakeDamage(_boomerangAttackDamage);
                break;

            }
        }
        else if(other.gameObject.CompareTag("Boss"))
        {
            int damage = AttackMode == KeyboardAttack.Melee ? _meleeAttackDamage :
                (AttackMode == KeyboardAttack.Boomerang ? _boomerangAttackDamage : 0);
            if(damage > 0) other.gameObject.GetComponentInParent<Boss>().TakeDamage(damage);
        }
        else if(AttackMode == KeyboardAttack.Boomerang && other.gameObject.layer == LayerMask.NameToLayer("Walls"))
        {
            _boomerangReturning = true;
            _rb.velocity = Vector2.zero;
        }

        
    }

}

public enum KeyboardAttack
{
    None, Melee, Boomerang, Shield
}
