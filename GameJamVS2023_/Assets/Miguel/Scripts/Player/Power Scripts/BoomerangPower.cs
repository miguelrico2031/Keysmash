using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Powers/Boomerang")]
public class BoomerangPower : Power
{
    [SerializeField] private int _damage;
    [SerializeField] private float _attackDuration, _initialSpeed, _rotationSpeed;


    private KeyboardAttack _lastState;

    public override void OnStart()
    {
        CoolDownOver = true;
    }

    public override void Use(GameObject player)
    {
        if(!CoolDownOver || Keyboard.Instance.AttackMode != KeyboardAttack.None) return;

        Keyboard.Instance.StartBoomerangAttack(_attackDuration, _initialSpeed, _rotationSpeed,_damage);
        BlockPowers = true;
        UsePower.Invoke(this);
    }

    public override void OnFixedUpdate()
    {
        if(Keyboard.Instance.AttackMode == KeyboardAttack.None)
        {
            BlockPowers = false;
            if(_lastState == KeyboardAttack.Boomerang) PowerAvailable.Invoke(this);
        }
        _lastState = Keyboard.Instance.AttackMode;
    }

    public override void OnUpdate()
    {
        
    }


    
}
