using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Powers/Melee")]
public class MeleePower : Power
{
    [SerializeField] private int _damage;
    [SerializeField] private float _attackDuration, _acceleration;

    
    public override void OnStart()
    {
        CoolDownOver = true;
    }

    public override void Use(GameObject player)
    {
        if(!CoolDownOver || Keyboard.Instance.AttackMode != KeyboardAttack.None) return;

        Keyboard.Instance.StartMeleeAttack(_attackDuration, _acceleration, _damage);
        BlockPowers = true;
        UsePower.Invoke(this);
    }

    public override void OnFixedUpdate()
    {
        if(Keyboard.Instance.AttackMode == KeyboardAttack.None) BlockPowers = false;
    }

    public override void OnUpdate()
    {
        
    }

    public override void OnCoolDownOver()
    {
        base.OnCoolDownOver();

        PowerAvailable.Invoke(this);
    }

}
