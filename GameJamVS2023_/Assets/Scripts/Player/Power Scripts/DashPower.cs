using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Powers/Dash")]
public class DashPower : Power
{
    [SerializeField] private float _initialSpeed;
    [SerializeField] private float _dashDuration;

    private Rigidbody2D _rb;
    private PlayerMovement _playerMovement;
    private float _speed, _acceleration;
    private Vector2 _direction;

    public bool Dashing = false;

    public override void Use(GameObject player)
    {
        if(!CoolDownOver || Dashing) return;


        _player = player;
        _rb = _player.GetComponent<Rigidbody2D>();
        Debug.Log(_rb);
        _playerMovement = player.GetComponent<PlayerMovement>();
        
        _direction = _playerMovement.Direction;

        GameObject.Find("AudioManager").GetComponent<AudioManager>().PlaySound("Dash");

        UsePower.Invoke(this);

        _playerMovement.BlockMovement = true;
        _speed = _initialSpeed;
        _acceleration = -_initialSpeed / _dashDuration;
        Dashing = true;
        BlockPowers = true;
        
    }

    public override void OnStart()
    {
        CoolDownOver = true;
        Dashing = false;
    }

    public override void OnUpdate()
    {
    }

    public override void OnFixedUpdate()
    {
        if(!Dashing) return;

        _speed += _acceleration * Time.fixedDeltaTime;
        _rb.velocity = _direction * _speed;

        if(_speed <= 0)
        {
            _rb.velocity = Vector2.zero;
            Dashing = false;
            _playerMovement.BlockMovement = false;
            BlockPowers = false;
        }

    }

    public override void OnCoolDownOver()
    {
        base.OnCoolDownOver();

        PowerAvailable.Invoke(this);
    }
}
