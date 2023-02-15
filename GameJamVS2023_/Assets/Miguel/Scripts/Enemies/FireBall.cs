using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    [HideInInspector] public int Damage;
    [HideInInspector] public float Speed, RotationSpeed, KnockbackForce, KnockbackDuration;

    private Rigidbody2D _rb;

    private GameObject _player;
    private StatsManager _playerStats;

    private Vector2 _directionToPlayer;

    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _playerStats = _player.GetComponent<StatsManager>();

        _rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        _directionToPlayer = (_player.transform.position - transform.position).normalized;

        _rb.velocity = _directionToPlayer * Speed * Time.fixedDeltaTime;
       
        float angle = Mathf.Atan2(_directionToPlayer.y, _directionToPlayer.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation =  Quaternion.RotateTowards(transform.rotation, rotation, RotationSpeed * Time.fixedDeltaTime);
        
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        Debug.Log(other.gameObject.name);
        if(other.CompareTag("Keyboard")) Destroy(gameObject);
        
        else if(other.gameObject.CompareTag("Player"))
        {
            _playerStats.TakeDamage(new EnemyAttack(Damage, _directionToPlayer * KnockbackForce, KnockbackDuration));
            Destroy(gameObject);
        }
        
        else if(other.gameObject.layer == LayerMask.NameToLayer("Walls")) Destroy(gameObject);        
    }

}
