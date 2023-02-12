using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StatsManager : MonoBehaviour
{
    public PlayerStats Stats;

    private PlayerMovement _playerMovement;
    private bool _isInvulnerable = false;

    public UnityEvent<int> HealthChange;

    void Awake()
    {
        _playerMovement = GetComponent<PlayerMovement>();
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
