using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerManager : MonoBehaviour
{
    public PlayerStats Stats;
    
    private Power _activePower;

    private void Start()
    {
        foreach(var power in Stats.Powers) power.OnStart();
    }
    
    private void Update()
    {
        if(_activePower && _activePower.BlockPowers) _activePower.OnUpdate();
        
        else
        {
            foreach(var power in Stats.Powers)
            {
                if(Input.GetKeyDown(power.Key))
                {
                    _activePower = power;
                    power.Use(gameObject);
                    if(power.CoolDownOver && power.CoolDown > 0) StartCoroutine(PowerCooldown(power));
                    break;
                }
            }

            foreach(var power in Stats.Powers) power.OnUpdate();
        }
        
        
    }

    private void FixedUpdate()
    {   
        if(_activePower && _activePower.BlockPowers) _activePower.OnFixedUpdate();

        else foreach(var power in Stats.Powers) power.OnFixedUpdate();
    }

    private IEnumerator PowerCooldown(Power power)
    {
        power.CoolDownOver = false;
        yield return new WaitForSeconds(power.CoolDown);
        power.CoolDownOver = true;
        power.OnCoolDownOver();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        foreach(var power in Stats.Powers) power.OnCollision(other.gameObject);
    }
}
