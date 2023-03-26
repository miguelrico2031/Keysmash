using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRoom : RoomManager
{
    public Boss BossPrefab;
    public Transform RoomCenter;

    public override void OnDungeonGenerated()
    {
        
        base.OnDungeonGenerated();
    }

    void SpawnBoss()
    {
        Instantiate(BossPrefab, RoomCenter.position, Quaternion.identity);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {

            CloseDoors();
            GameObject.Find("AudioManager").GetComponent<AudioManager>().PlaySound("BossMusic");
            GameObject.Find("Canvas").GetComponent<AudioSource>().volume = 0;
            Invoke("SpawnBoss", 3f);
        }
    }
}
