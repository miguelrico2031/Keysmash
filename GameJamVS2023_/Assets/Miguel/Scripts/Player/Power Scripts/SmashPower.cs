using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 using UnityEngine.Rendering.Universal;

[CreateAssetMenu(menuName = "Powers/Smash")]

public class SmashPower : Power
{
    [SerializeField] private int _damage;
    [SerializeField] private DashPower _dashPower;

    private StatsManager _statsManager;
    private Light2D _light;

    public override void OnStart()
    {
        _dashPower.PowerAvailable.AddListener(OnDashOver);
        _statsManager = GameObject.FindGameObjectWithTag("Player").GetComponent<StatsManager>();
        _light =  GameObject.FindGameObjectWithTag("Player").GetComponent<Light2D>();
    }

    public override void Use(GameObject player)
    {
        //if(!_statsManager) _statsManager = GameObject.FindGameObjectWithTag("Player").GetComponent<StatsManager>();
        _statsManager.MakeInvulnerable(0.3f);
        _light.enabled = true;
        UsePower.Invoke(this);
    }

    public override void OnFixedUpdate()
    {
        if(!_statsManager.IsInvulnerable) _light.enabled = false;
    }

    public override void OnUpdate()
    {
        
    }

    public void OnDashOver(Power p)
    {
        PowerAvailable.Invoke(this);

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
