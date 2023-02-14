using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyRoom : RoomManager
{
    private List<EnemySpawnPoint> _enemySpawnPoints;

    private int _enemyCount;
    public override void OnDungeonGenerated()
    {
        base.OnDungeonGenerated();
        
        _enemySpawnPoints = new List<EnemySpawnPoint>();
        foreach(var spawnPoint in GetComponentsInChildren<EnemySpawnPoint>())
        {
            _enemySpawnPoints.Add(spawnPoint);
            if(spawnPoint.IsAtStart) SpawnEnemy(spawnPoint);
        }
        _enemyCount = _enemySpawnPoints.Count;

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            CloseDoors();
            SpawnEnemies();
        }
        // else if(other.gameObject.CompareTag("Enemy"))
        // {
        //     var lizard = other.gameObject.GetComponent<Lizard>();
        //     if(lizard && !lizard.RoomBound) lizard.RoomBound = GetComponent<BoxCollider2D>();
        // }
    }

    private void SpawnEnemies()
    {
        foreach(var spawnPoint in _enemySpawnPoints)
        {
            if(!spawnPoint.IsSpawned)
            {
                StartCoroutine(SpawnEnemy(spawnPoint));
            }
        }
    }

    private IEnumerator SpawnEnemy(EnemySpawnPoint spawnPoint)
    {
        yield return new WaitForSeconds(spawnPoint.SpawnDelay);
        Enemy enemy = Instantiate(spawnPoint.EnemyToSpawn, spawnPoint.transform.position, Quaternion.identity);
        enemy.OnDie.AddListener(OnEnemyDie);
        spawnPoint.IsSpawned = true;
    }

    public void OnEnemyDie(Enemy enemy)
    {
        _enemyCount --;
        if(_enemyCount == 0) OpenDoors();
    }

}
