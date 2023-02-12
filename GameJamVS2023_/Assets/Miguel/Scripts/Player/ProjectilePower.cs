using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Powers/Projectile")]
public class ProjectilePower : Power
{
    [SerializeField] private GameObject _keyCapPrefab;
    [SerializeField] private int _damage;
    [SerializeField] private float _speed;
    [SerializeField] private float _harmlessSpeed;
    
    private bool _hasKeyCap;
    private KeyCap _keyCap;

    private PlayerMovement _playerMovement;
    private Vector2 _direction;

    public override void Use(GameObject player)
    {
        if(!_hasKeyCap) return;


        _player = player;
        _playerMovement = player.GetComponent<PlayerMovement>();
    
        _direction = _playerMovement.Direction;

        ThrowKeyCap();
    }    

    public override void OnStart()
    {
        _hasKeyCap = true;
        _keyCap = Instantiate(_keyCapPrefab, Vector3.zero, Quaternion.identity).GetComponent<KeyCap>();
        _keyCap.ProjectilePower = this;
        _keyCap.Speed = _speed;
        _keyCap.HarmlessSpeed = _harmlessSpeed;
        _keyCap.Damage = _damage;
        _keyCap.gameObject.SetActive(false);
    }

    public override void OnUpdate()
    {
        
    }
    public override void OnFixedUpdate()
    {
        
    }

    void ThrowKeyCap()
    {
        _hasKeyCap = false;
       // _keyCap.transform.position = Keyboard.Instance.transform.position + (Vector3)_direction * 0.2f;
        _keyCap.transform.position = _player.transform.position + (Vector3) _direction * 0.4f;
        _keyCap.gameObject.SetActive(true);
        _keyCap.Throw(_direction);

    }

    public void GetKeyCap()
    {
        _keyCap.gameObject.SetActive(false);
        _hasKeyCap = true;
    }
    
}
