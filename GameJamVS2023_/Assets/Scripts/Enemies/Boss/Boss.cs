    using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Boss : MonoBehaviour
{
   public BossState State;
    public int MaxHealth, Health;

    [SerializeField] private float  _minSlashTime, _maxSlashTime;
    public int SlashDamage, SlashAttackNumber;
    public float SlashKnockback, SlashKnockbackDuration;


    public int ThrustDamage;
    public float ThrustKnockback, ThrustKnockbackDuration;
    [SerializeField] private float  _thrustDelay;

    public int FireballDamage, FireballRounds;
    public float FireballDelay, FireballRoundDelay,FireballSpeed, FireballRotationSpeed, FireballKnockbackForce, FireballKnockbackDuration;

    public float VulnerabilityDuration, DeadDuration, StartDelay;

    private List<Slash> _slashes;
    [SerializeField] private Thrust _leftThrust, _rightThrust;
    [SerializeField] private BossHead _head;

    [SerializeField] GameObject[] _keys;
    public SpriteRenderer Head;


    private DamageAnimation _damageAnim;

    private bool _hasPlayerAttacked = false;
    

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
        Invoke("Slash", StartDelay);
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
        
    }

    IEnumerator VulnerabilityTime()
    {
        yield return new WaitForSeconds(VulnerabilityDuration);
        if(State != BossState.Dead)
        {
            Slash();
            _hasPlayerAttacked = false;
        }
        
    }

    public void TakeDamage(int damage)
    {
        if(State != BossState.Vulnerable) return;

        Health --;

        if(!_hasPlayerAttacked)
        {
            _hasPlayerAttacked = true;
            StartCoroutine(VulnerabilityTime());
        }

        _damageAnim.StartAnimation();
        if(Health <= 0)
        {
            Debug.Log("health 0");
            _head.DieAnimation();
            State = BossState.Dead;

            Invoke("BossDead", DeadDuration);
        }
    }

    public void BossDead()
    {
        StartCoroutine(EndCredits());
    }


    IEnumerator EndCredits()
    {
        foreach (var key in _keys)
        {
            yield return new WaitForSeconds(0.03f);
            GameObject thisKey = Instantiate(key, Head.transform.position - new Vector3(0, 0.5f, 0), Quaternion.identity);
            thisKey.transform.localScale = thisKey.transform.localScale * 0.8f;
            Vector2 dir = new Vector2(Random.Range(-0.9f, 0.9f), -1);
            Debug.Log(dir);
            thisKey.GetComponent<Rigidbody2D>().velocity =  dir.normalized * Random.Range(10, 26);
        }
        yield return new WaitForSeconds(3);
        GameObject.Find("Canvas").GetComponent<Interfaz>().FinalScreen();
        yield return new WaitForSeconds(5);
        SceneManager.LoadScene("Credits");
    }

}

public enum BossState
{
    Slash, Thrust, BulletHell, Vulnerable, Dead
}
