using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PlayerStats")]
public class PlayerStats : ScriptableObject
{
    public List<Power> Powers;
    public int MaxHealth;
    public float _invulnerabilityDuration;
    public int Health; /*{get; private set;}*/

    public void DamagePlayer(int damage)
    {
        Health -= damage;

        if(Health <= 0)
        {
            //Die
        }
    }

    public void HealPlayer(int healAmount)
    {
        Health = Mathf.Clamp(Health + healAmount, 0, MaxHealth);
    }

}