using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Powers/Bomb")]
public class BombPower : Power
{
    [SerializeField] private GameObject _keyBombPrefab;
    [SerializeField] private int _damage;
    [SerializeField] private float _bombActivationDuration;
    
    private bool _hasKeyBomb;
    private KeyBomb _keyBomb;

    private PlayerMovement _playerMovement;
    private Vector2 _direction;

    public override void Use(GameObject player)
    {
        if(!_hasKeyBomb) return;


        _player = player;
        _playerMovement = player.GetComponent<PlayerMovement>();
    
        _direction = _playerMovement.Direction;

        UsePower.Invoke(this);

        ThrowKeyCap();
    }    
    public override void OnStart()
    {
        _hasKeyBomb = true;
        GameObject cap = Instantiate(_keyBombPrefab, Vector3.zero, Quaternion.identity);
        _keyBomb = cap.GetComponent<KeyBomb>();
        _keyBomb.BombPower = this;
        _keyBomb.Damage = _damage;
        _keyBomb.gameObject.SetActive(false);
    }

    public override void OnUpdate()
    {
        
    }
    public override void OnFixedUpdate()
    {
        
    }

    void ThrowKeyCap()
    {
        _hasKeyBomb = false;

        _keyBomb.transform.position = _player.transform.position;
        _keyBomb.gameObject.SetActive(true);
        _keyBomb.ActivateBomb(_bombActivationDuration);

    }

    public void GetBombKey()
    {
        _keyBomb.gameObject.SetActive(false);
        _hasKeyBomb = true;
        PowerAvailable.Invoke(this);
    }
    
}