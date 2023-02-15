using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Powers/Smash")]

public class SmashPower : Power
{
    [SerializeField] private int _damage;
    [SerializeField] private DashPower _dashPower;

    private StatsManager _statsManager;

    public override void OnStart()
    {
        _dashPower.PowerAvailable.AddListener(OnDashOver);
        _statsManager = GameObject.FindGameObjectWithTag("Player").GetComponent<StatsManager>();
    }

    public override void Use(GameObject player)
    {
        //if(!_statsManager) _statsManager = GameObject.FindGameObjectWithTag("Player").GetComponent<StatsManager>();
        _statsManager.IsInvulnerable = true;
        UsePower.Invoke(this);
    }

    public override void OnFixedUpdate()
    {
        
    }

    public override void OnUpdate()
    {
        
    }

    public void OnDashOver(Power p)
    {
        PowerAvailable.Invoke(this);
        _statsManager.IsInvulnerable = false;
    }

    public override void OnCollision(GameObject other)
    {
        base.OnCollision(other);

        if(!_dashPower.Dashing) return;

        if(other.CompareTag("Enemy"))
        {
            other.gameObject.GetComponent<Enemy>().TakeDamage(_damage);
        }
        else if(other.CompareTag("Boss"))
        {
            other.gameObject.GetComponentInParent<Boss>().TakeDamage(_damage);
        }
    }
}
