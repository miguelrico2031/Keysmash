    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
   [HideInInspector] public BossState State;
    public int MaxHealth, Health;

    [SerializeField] private float  _minSlashTime, _maxSlashTime;
    public int SlashDamage, SlashAttackNumber;
    public float SlashKnockback, SlashKnockbackDuration;


    public int ThrustDamage;
    public float ThrustKnockback, ThrustKnockbackDuration;
    [SerializeField] private float  _thrustDelay;

    public int FireballDamage, FireballRounds;
    public float FireballDelay, FireballRoundDelay,FireballSpeed, FireballRotationSpeed, FireballKnockbackForce, FireballKnockbackDuration;

    public float VulnerabilityDuration;

    private List<Slash> _slashes;
    [SerializeField] private Thrust _leftThrust, _rightThrust;
    [SerializeField] private BossHead _head;
    private DamageAnimation _damageAnim;
    

    public StatsManager PlayerStats;

    void Awake()
    {
        PlayerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<StatsManager>();

        _slashes = new List<Slash>();
        for (int i = 0; i < transform.childCount; i++)
        {
            if(transform.GetChild(i).CompareTag("Slash")) _slashes.Add(transform.GetChild(i).GetComponent<Slash>());
        }
        Health = MaxHealth;
        _damageAnim = GetComponent<DamageAnimation>();
    }

    void Start()
    {
        Slash();
    }

    void Slash()
    {
        State = BossState.Slash;
        StartCoroutine(SlashTime());
    }

    IEnumerator SlashTime()
    {
        _head.StartSlash();
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
        State = BossState.Thrust;
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
        State = BossState.BulletHell;
        _head.StartBulletHell();
        
    }

    public void OnBulletHellOver()
    {
        _rightThrust.EndThrustAttack();
        _leftThrust.EndThrustAttack();
        Vulnerability();
    }

    void Vulnerability()
    {
        State = BossState.Vulnerable;
        StartCoroutine(VulnerabilityTime());
    }

    IEnumerator VulnerabilityTime()
    {
        yield return new WaitForSeconds(VulnerabilityDuration);
        Slash();
    }

    public void TakeDamage(int damage)
    {
        if(State != BossState.Vulnerable) return;

        Health --;
        _damageAnim.StartAnimation();
        if(Health <= 0)
        {
            //Die
        }
    }

}

public enum BossState
{
    Slash, Thrust, BulletHell, Vulnerable
}
