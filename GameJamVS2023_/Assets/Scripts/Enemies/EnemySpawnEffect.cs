using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnEffect : MonoBehaviour
{
    public EnemyRoom Room;

    public Enemy Enemy;

    private Enemy _instance;


    public void OnEffectEnd()
    {
        _instance = Instantiate(Enemy, transform.position, Quaternion.identity);
        Room.OnEnemySpawn(_instance);
        Destroy(gameObject);
    }
}
