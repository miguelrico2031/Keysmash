using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DungeonStart : MonoBehaviour
{ 
    void Start()
    {
        if (SceneManager.GetActiveScene().name == "Level 1")
        {
            StatsManager stats = GetComponent<StatsManager>();
            stats.Stats.Health = 0;
            stats.HealPlayer(stats.Stats.StartingLives);
        }
    }
}
