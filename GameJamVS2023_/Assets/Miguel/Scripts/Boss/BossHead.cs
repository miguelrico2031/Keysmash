using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHead : MonoBehaviour
{
    [SerializeField] private PolygonCollider2D _openCollider, _closeCollider;
    [SerializeField] private FireBall _fireball;

    private GameObject _player;

    List<FireBall> _fireballs;

    int fireballNumber;
    Vector2 _directionToPlayer;

    private Boss _boss;

    void Start()
    {
        _fireballs = new List<FireBall>();
        _openCollider.enabled = false;
        _closeCollider.enabled = true;

        _player = GameObject.FindGameObjectWithTag("Player");

        _boss = GetComponentInParent<Boss>();

    }

    public void StartBulletHell()
    {
        fireballNumber = _boss.FireballRounds * transform.childCount;
        _fireballs = new List<FireBall>();
        _openCollider.enabled = true;
        _closeCollider.enabled = false;
        StartCoroutine(BulletHell());
    }

    IEnumerator BulletHell()
    {
        for (int k = 0; k < _boss.FireballRounds; k++)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                _directionToPlayer = (_player.transform.position - transform.GetChild(i).position).normalized;
                FireBall fireballInstance = Instantiate(_fireball, transform.GetChild(i).position,
                Quaternion.AngleAxis(Mathf.Atan2(_directionToPlayer.y, _directionToPlayer.x) * Mathf.Rad2Deg, Vector3.forward));
                _fireballs.Add(fireballInstance);

                fireballInstance.Damage = _boss.FireballDamage;
                fireballInstance.Speed = _boss.FireballSpeed;
                fireballInstance.RotationSpeed = _boss.FireballRotationSpeed;
                fireballInstance.KnockbackForce = _boss.FireballKnockbackForce;
                fireballInstance.KnockbackDuration = _boss.FireballKnockbackDuration;

                yield return new WaitForSeconds(_boss.FireballDelay);
            }
            yield return new WaitForSeconds(_boss.FireballRoundDelay);          
        }

    }

    // void FixedUpdate()
    // {
    //     bool phaseOver = true;
    //     foreach (var fireball in _fireballs)
    //     {
    //         if(fireball)
    //         {
    //             phaseOver = false;
    //             break;
    //         }
    //     }
        
    //     if(phaseOver) _boss.OnBulletHellOver();
        
    // }

    public void OnFireballDestroy()
    {
        fireballNumber --;
        if(fireballNumber <= 0f) _boss.OnBulletHellOver();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            _directionToPlayer = (_player.transform.position - transform.position).normalized;
            other.gameObject.GetComponent<StatsManager>().
                TakeDamage(_boss.FireballDamage, _directionToPlayer * _boss.FireballKnockbackForce, _boss.FireballKnockbackDuration);
            
            
        }
    }
}
