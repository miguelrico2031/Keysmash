    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [SerializeField] private float  _minSlashTime, _maxSlashTime;
    public int SlashDamage, SlashAttackNumber;
    public float SlashKnockback, SlashKnockbackDuration;


    public int ThrustDamage;
    public float ThrustKnockback, ThrustKnockbackDuration;
    [SerializeField] private float  _thrustDelay;

    public int FireballDamage, FireballRounds;
    public float FireballDelay, FireballRoundDelay,FireballSpeed, FireballRotationSpeed, FireballKnockbackForce, FireballKnockbackDuration;

    private List<Slash> _slashes;
    [SerializeField] private Thrust _leftThrust, _rightThrust;
    [SerializeField] private BossHead _head;
    private BossState _state;

    public StatsManager PlayerStats;

    void Awake()
    {
        PlayerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<StatsManager>();

        _slashes = new List<Slash>();
        for (int i = 0; i < transform.childCount; i++)
        {
            if(transform.GetChild(i).CompareTag("Slash")) _slashes.Add(transform.GetChild(i).GetComponent<Slash>());
        }
    }

    void Start()
    {
        Slash();
    }

    void Slash()
    {
        _state = BossState.Slash;
        StartCoroutine(SlashTime());
    }

    IEnumerator SlashTime()
    {
        for (int i = 0; i < SlashAttackNumber; i++)
        {
            SlashAttack();
            yield return new WaitForSeconds(Random.Range(_minSlashTime, _maxSlashTime));
        }
        Thrust();
    }

    void SlashAttack()
    {
        _slashes[Random.Range(0, _slashes.Count)].SlashAttack();
    }

    void Thrust()
    {
        _state = BossState.Thrust;
        StartCoroutine(ThrustTime());
    }

    IEnumerator ThrustTime()
    {
        Thrust first, second;
        if(Random.value > 0.5f) 
        {
            first = _rightThrust;
            second = _leftThrust;
        }
        else
        {
            first = _rightThrust;
            second = _leftThrust;
        }
        yield return new WaitForSeconds(_thrustDelay);
        ThrustAttack(first);
        yield return new WaitForSeconds(_thrustDelay);
        ThrustAttack(second);

        BulletHell();

    }

    void ThrustAttack(Thrust thrust)
    {
        thrust.ThrustAttack();
    }

    void BulletHell()
    {
        _state = BossState.BulletHell;
        _head.StartBulletHell();
        
    }

    public void OnBulletHellOver()
    {
        _rightThrust.EndThrustAttack();
        _leftThrust.EndThrustAttack();
    }

}

public enum BossState
{
    Slash, Thrust, BulletHell, Vulnerable
}
