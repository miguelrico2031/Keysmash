using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Keyboard : MonoBehaviour
{
    public static Keyboard Instance;

    public bool Attacking = false;

    [SerializeField] private Transform _rightHand;

    private BoxCollider2D _collider;
    private HandsFollowPlayer _rightHandFollowPlayer;
    private PlayerMovement _playerMovement;

    private float attackTime;
    void Awake()
    {
        Instance = this;
        _collider = GetComponent<BoxCollider2D>();
        _rightHandFollowPlayer = _rightHand.GetComponent<HandsFollowPlayer>();
    }
    private void Start()
    {
        _playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        _collider.enabled = false;
    }

    
    void FixedUpdate()
    {
        transform.position = _rightHand.position;

        if(!Attacking) return;

        if(!_rightHandFollowPlayer.IsInPlace) return;

        _rightHandFollowPlayer.IsBlocked = true;
        //block rotatehands?
        _playerMovement.BlockMovement = true;

    }

    public void MeleeAttack(float attackDuration, float attackDamage)
    {
        //make the right hand rotate aroun the player head
        //enable the collider and disable it when the movement is done
        Attacking = true;
    }

    private void CalculateAttackPoints()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.CompareTag("Enemy"))
        {
            //auchearlo joder
        }
    }

}
