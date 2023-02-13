using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StatsManager : MonoBehaviour
{
    public PlayerStats Stats;
    public bool IsCovering = false;

    private PlayerMovement _playerMovement;
    private bool _isInvulnerable = false;

    public UnityEvent<int> HealthChange;

    public List<EnemyAttack> ParriedAttacks;

    void Awake()
    {
        _playerMovement = GetComponent<PlayerMovement>();
        ParriedAttacks = new List<EnemyAttack>();
    }

    public void TakeDamage(int damage)
    {
        if (_isInvulnerable) return;
        Stats.DamagePlayer(damage);
        HealthChange.Invoke(-damage);
    }

    public void TakeDamage(int damage, Vector2 knockBackForce, float duration)
    {
        if (_isInvulnerable) return;
        Stats.DamagePlayer(damage);
        _playerMovement.AddKnockback(knockBackForce, duration);
        HealthChange.Invoke(-damage);
    }

    public void TakeDamage(EnemyAttack attack)
    {
        if(ParriedAttacks.Contains(attack))
        {
            ParriedAttacks.Remove(attack);
            return;
        }

        TakeDamage(attack.damage, attack.knockbackForce, attack.knockbackDuration);
    }

    public void HealPlayer(int healAmount)
    {
        Stats.HealPlayer(healAmount);
        HealthChange.Invoke(healAmount);
    }

    private IEnumerator InvulnerabilityTime(float duration)
    {
        _isInvulnerable = true;
        yield return new WaitForSeconds(duration);
        _isInvulnerable = false;
    }
}

