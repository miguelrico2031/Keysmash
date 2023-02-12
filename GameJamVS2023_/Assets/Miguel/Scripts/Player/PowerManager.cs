using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerManager : MonoBehaviour
{
    public PlayerStats Stats;

    private void Start()
    {
        foreach(var power in Stats.Powers) power.OnStart();
    }
    
    private void Update()
    {
        foreach(var power in Stats.Powers)
        {
            if(Input.GetKeyDown(power.Key))
            {
                power.Use(gameObject);
                if(power.CoolDownOver && power.CoolDown > 0) StartCoroutine(PowerCooldown(power));
                break;
            }
        }

        foreach(var power in Stats.Powers) power.OnUpdate();
        
    }

    private void FixedUpdate()
    {
        foreach(var power in Stats.Powers) power.OnFixedUpdate();
    }

    private IEnumerator PowerCooldown(Power power)
    {
        power.CoolDownOver = false;
        yield return new WaitForSeconds(power.CoolDown);
        power.CoolDownOver = true;
    }
}
