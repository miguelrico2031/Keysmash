using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PlayerStats")]
public class PlayerStats : ScriptableObject
{
    public int StartingLives;
    public List<Power> Powers;
    public int MaxHealth;
    public float InvulnerabilityDuration;
    public int Health; /*{get; private set;}*/

    public void DamagePlayer(int damage)
    {
        Health -= damage;

        if(Health <= 0)
        {
            GameObject.Find("Canvas").GetComponent<DieEvent>().OnDeath();
        }
    }

    public void HealPlayer(int healAmount)
    {
        Health = Mathf.Clamp(Health + healAmount, 0, MaxHealth);
    }

}