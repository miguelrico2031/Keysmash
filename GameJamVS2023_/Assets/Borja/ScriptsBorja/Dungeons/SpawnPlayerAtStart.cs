using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPlayerAtStart : MonoBehaviour
{
    private GameObject _player;
    Vector2 _spawnPoint;
    private void Start()
    {
        DungeonManager.Instance.DungeonGenerated.AddListener(OnDungeonGenerated);
    }

    public void OnDungeonGenerated()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _spawnPoint = GameObject.Find("PlayerSpawnPoint").transform.position;
        Debug.Log(_spawnPoint);
        _player.transform.position = _spawnPoint;
    }
}
