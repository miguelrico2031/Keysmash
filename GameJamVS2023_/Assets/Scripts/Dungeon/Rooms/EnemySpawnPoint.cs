using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnPoint : MonoBehaviour
{
    public EnemySpawnEffect SpawnEffect;
    public Enemy EnemyToSpawn;
    public float SpawnDelay;
    public bool IsAtStart;
    public bool IsSpawned = false;
}
