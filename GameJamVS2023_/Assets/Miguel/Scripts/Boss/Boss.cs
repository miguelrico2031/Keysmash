    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [SerializeField] private float  _minSlashTime, _maxSlashTime;
    public int SlashDamage, SlashAttackNumber, ThrustDamage;


    private List<Slash> _slashes;
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
    }

    void SlashAttack()
    {
        _slashes[Random.Range(0, _slashes.Count)].SlashAttack();
    }

}

public enum BossState
{
    Slash, Wait, Thrust, Vulnerable
}
