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
                    
                    if(power is DashPower)
                    {
                        DashPower dashPower = (DashPower) power;
                        if(dashPower.CoolDownOver && !dashPower.Dashing)
                        {
                            dashPower.Use(gameObject);
                            foreach(var smash in Stats.Powers)
                            {
                                if(smash is SmashPower)
                                {
                                    smash.Use(gameObject);
                                }
                            }
                        }

                    }
                    else if(!(power is SmashPower)) power.Use(gameObject);
                    if(!power.ManualCooldown && power.CoolDownOver && power.CoolDown > 0) StartCooldown(power);
                    else if(power.ManualCooldown) power.StartCooldown.AddListener(StartCooldown);
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

    public void StartCooldown(Power power)
    {
        StartCoroutine(PowerCooldown(power));
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
