using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Powers/Melee")]
public class MeleePower : Power
{
    [SerializeField] private int _damage;
    [SerializeField] private float _attackDuration;

    
    public override void OnStart()
    {
        CoolDownOver = true;
    }

    public override void Use(GameObject player)
    {
        if(!CoolDownOver || Keyboard.Instance.Attacking) return;
    }

    public override void OnFixedUpdate()
    {
        
    }

    public override void OnUpdate()
    {
        
    }

}
