using UnityEngine;
using UnityEngine.Events;

public abstract class Power : ScriptableObject
{
    public GameObject UIPrefab, HUDPrefab, InfoPrefab;
    public float CoolDown;
    [HideInInspector] public bool CoolDownOver = true;
    [HideInInspector] public bool ManualCooldown = false;
    [HideInInspector] public bool BlockPowers = false;
    public string Key;

    [TextArea] public string Description;

    public UnityEvent<Power> UsePower, PowerAvailable, StartCooldown;

    protected GameObject _player;
    


    public abstract void Use(GameObject player);

    public abstract void OnStart();
    public abstract void OnUpdate();

    public abstract void OnFixedUpdate();
    public virtual void OnCoolDownOver()
    {
        
    }

    public virtual void OnCollision(GameObject other)
    {

    }



}



