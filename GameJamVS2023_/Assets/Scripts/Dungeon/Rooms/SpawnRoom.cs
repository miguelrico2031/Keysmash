using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnRoom : RoomManager
{
    Transform _player;

    // public override void Awake()
    // {
    //     base.Awake();

    // }

    public override void OnDungeonGenerated()
    {
        base.OnDungeonGenerated();

        _player = GameObject.FindGameObjectWithTag("Player").transform.parent;
        _player.position = GameObject.Find("PlayerSpawnPoint").transform.position;

        Invoke("OpenDoors", 0.2f);
    }
}
