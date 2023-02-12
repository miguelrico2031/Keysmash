using UnityEngine;

public abstract class Power : ScriptableObject
{
    public Sprite UISprite;
    public float CoolDown;
    [HideInInspector] public bool CoolDownOver = true;
    public string Key;
    protected GameObject _player;

    public abstract void Use(GameObject player);

    public abstract void OnStart();
    public abstract void OnUpdate();

    public abstract void OnFixedUpdate();


}
