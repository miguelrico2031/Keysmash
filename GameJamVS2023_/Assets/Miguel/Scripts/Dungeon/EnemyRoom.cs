using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyRoom : RoomManager
{
    [SerializeField] private GameObject _stairs;

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

        if (_stairs) { _stairs.SetActive(false); }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("aaaaaaa");
        if (other.gameObject.CompareTag("Player"))
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
        if(_enemyCount == 0) RoomFinished();
    }

    private void RoomFinished()
    {
        OpenDoors();
        if(!_stairs) return;

        _stairs.SetActive(true);
    }

}
