using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Powers/Shield")]

public class ShieldPower : Power
{
    [SerializeField] private float _bashDuration; 

    public override void OnStart()
    {
        CoolDownOver = true;
    }

    public override void Use(GameObject player)
    {
        if(!CoolDownOver || Keyboard.Instance.AttackMode != KeyboardAttack.None) return;

        Keyboard.Instance.StartShieldCover(_bashDuration);
        BlockPowers = true;
        UsePower.Invoke(this);
    }

    public override void OnFixedUpdate()
    {
        if(Keyboard.Instance.AttackMode == KeyboardAttack.None) BlockPowers = false;
    }

    
    public override void OnUpdate()
    {
        if(Keyboard.Instance.AttackMode == KeyboardAttack.Shield && Input.GetKeyUp(Key))
        {
            Keyboard.Instance.EndShieldCover();
        }
    }

    public override void OnCoolDownOver()
    {
        base.OnCoolDownOver();

        PowerAvailable.Invoke(this);
    }
}
