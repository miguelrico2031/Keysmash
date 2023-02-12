using UnityEngine;
using UnityEngine.Events;

public abstract class Power : ScriptableObject
{
    public GameObject UIPrefab, HUDPrefab;
    public float CoolDown;
    [HideInInspector] public bool CoolDownOver = true;
    [HideInInspector] public bool BlockPowers = false;
    public string Key;

    [TextArea] public string Description;

    //descripcion de lo que hace el objeto
    //conseguir una tecla
    public UnityEvent<Power> UsePower, PowerAvailable;

    protected GameObject _player;
    


    public abstract void Use(GameObject player);

    public abstract void OnStart();
    public abstract void OnUpdate();

    public abstract void OnFixedUpdate();
    public virtual void OnCoolDownOver()
    {
        
    }


}



