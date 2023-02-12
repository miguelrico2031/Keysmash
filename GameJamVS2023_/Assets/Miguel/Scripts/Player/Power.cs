using UnityEngine;

public abstract class Power : ScriptableObject
{
    public GameObject UIPrefab, HUDPrefab;
    public float CoolDown;
    [HideInInspector] public bool CoolDownOver = true;
    [HideInInspector] public bool BlockPowers = false;
    public string Key;
    protected GameObject _player;


    public abstract void Use(GameObject player);

    public abstract void OnStart();
    public abstract void OnUpdate();

    public abstract void OnFixedUpdate();


}
