using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthRoom : RoomManager
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            OpenDoors();
        }

    }
}
