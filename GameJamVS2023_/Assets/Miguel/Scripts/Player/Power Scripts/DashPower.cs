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
    private bool _dashing = false;
    private float _speed, _acceleration;
    private Vector2 _direction;

    public override void Use(GameObject player)
    {
        if(!CoolDownOver || _dashing) return;


        _player = player;
        _rb = _player.GetComponent<Rigidbody2D>();
        _playerMovement = player.GetComponent<PlayerMovement>();
        
        _direction = _playerMovement.Direction;

        UsePower.Invoke(this);

        Dash();
        
    }

    public override void OnStart()
    {
        CoolDownOver = true;
    }

    public override void OnUpdate()
    {
    }

    public override void OnFixedUpdate()
    {
        if(!_dashing) return;

        _speed += _acceleration * Time.fixedDeltaTime;
        _rb.velocity = _direction * _speed;

        if(_speed <= 0)
        {
            _rb.velocity = Vector2.zero;
            _dashing = false;
            _playerMovement.BlockMovement = false;
            BlockPowers = false;
        }

    }

    private void Dash()
    {
        _playerMovement.BlockMovement = true;
        _speed = _initialSpeed;
        _acceleration = -_initialSpeed / _dashDuration;
        _dashing = true;
        BlockPowers = true;

    }

    public override void OnCoolDownOver()
    {
        base.OnCoolDownOver();

        PowerAvailable.Invoke(this);
    }
}
