using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPlayerAtStart : MonoBehaviour
{
    [SerializeField] private GameObject _player;
    Vector2 _spawnPoint;
    private void Start()
    {
        DungeonManager.Instance.DungeonGenerated.AddListener(OnDungeonGenerated);
    }

    public void OnDungeonGenerated()
    {
        
        _spawnPoint = GameObject.Find("PlayerSpawnPoint").transform.position;
        Debug.Log(_spawnPoint);
        Instantiate(_player, _spawnPoint, Quaternion.identity);
    }
}
