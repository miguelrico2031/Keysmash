using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StatsManager : MonoBehaviour
{
    public PlayerStats Stats;
    public bool IsInvulnerable = false;

    private PlayerMovement _playerMovement;
    private DamageAnimation _damageAnimation;
    

    public UnityEvent<int> HealthChange;

    //public List<EnemyAttack> ParriedAttacks;

    void Awake()
    {
        _playerMovement = GetComponent<PlayerMovement>();
        _damageAnimation = GetComponent<DamageAnimation>();
        //ParriedAttacks = new List<EnemyAttack>();
    }

    public void TakeDamage(int damage)
    {
        if (IsInvulnerable) return;
        Stats.DamagePlayer(damage);
        HealthChange.Invoke(-damage);
        GameObject.Find("Canvas").GetComponent<Interfaz>().ChangeLives(-damage);
        StartCoroutine(InvulnerabilityTime(Stats.InvulnerabilityDuration));
    }

    public void TakeDamage(int damage, Vector2 knockBackForce, float duration)
    {
        if (IsInvulnerable) return;
        Stats.DamagePlayer(damage);
        _damageAnimation.StartAnimation();
        _playerMovement.AddKnockback(knockBackForce, duration);
        HealthChange.Invoke(-damage);
        GameObject.Find("Canvas").GetComponent<Interfaz>().ChangeLives(-damage);
        StartCoroutine(InvulnerabilityTime(Stats.InvulnerabilityDuration));
    }

    public void TakeDamage(EnemyAttack attack)
    {
        // if(ParriedAttacks.Contains(attack))
        // {
        //     ParriedAttacks.Remove(attack);
        //     return;
        // }

        TakeDamage(attack.damage, attack.knockbackForce, attack.knockbackDuration);
    }

    public void HealPlayer(int healAmount)
    {
        Stats.HealPlayer(healAmount);
        HealthChange.Invoke(healAmount);
        GameObject.Find("Canvas").GetComponent<Interfaz>().ChangeLives(healAmount);
    }

    private IEnumerator InvulnerabilityTime(float duration)
    {
        IsInvulnerable = true;
        yield return new WaitForSeconds(duration);
        IsInvulnerable = false;
    }
}

