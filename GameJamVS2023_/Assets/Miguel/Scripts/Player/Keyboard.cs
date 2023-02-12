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
    private HandsFollowPlayer _rightHandFollowPlayer, _leftHandFollowPlayer;
    private PlayerMovement _playerMovement;

    private float _meleeAttackTime, _meleeAttackDuration, _meleeAcceleration;
    private int _meleeAttackDamage;


    private Vector2 _meleeStartPos, _meleeMiddlePos, _meleeEndPos;

    private bool _boomerangReturning = false;
    private Vector2 _boomerangDirection;
    private float _boomerangAcceleration, _boomerangSpeed, _boomerangAttackDuration, _boomerangAttackTime, _boomerangRotationSpeed;
    private int _boomerangAttackDamage;

    void Awake()
    {
        Instance = this;
        _collider = GetComponent<BoxCollider2D>();
        _rb = GetComponent<Rigidbody2D>();
        _rightHandFollowPlayer = _rightHand.GetComponent<HandsFollowPlayer>();
        _leftHandFollowPlayer = _leftHand.GetComponent<HandsFollowPlayer>();
    }
    private void Start()
    {
        _playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
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
        //block rotatehands?
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


        _rightHand.position = Bezier.GetBezier(_meleeAttackTime, _meleeAttackDuration, _meleeStartPos, _meleeMiddlePos, _meleeEndPos);

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


    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Enemy"))
        {
            var enemy = other.gameObject.GetComponent<Enemy>();
            int damage = 0;
            switch(AttackMode)
            {
                case KeyboardAttack.Melee:
                damage = _meleeAttackDamage;
                break;
                
                case KeyboardAttack.Boomerang:
                damage = _boomerangAttackDamage;
                break;
        }
            enemy.TakeDamage(damage);
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
    None, Melee, Boomerang
}
