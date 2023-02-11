using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Edgar.Unity;

public class RoomManager : MonoBehaviour 
{
    // Start is called before the first frame update
    public bool IsSpawn;

    //[SerializeField] private List<Enemy> enemies;

    private RoomInfoGrid2D _roomInfo;
    private RoomInstanceGrid2D _roomInstance;
    private List<CorridorManager> _corridors;


    private void Awake()
    {
        _corridors = new List<CorridorManager>();
    }
    private void OnEnable()
    {
        DungeonManager.Instance.DungeonGenerated.AddListener(OnDungeonGenerated);
    }

    private void OnDisable()
    {
        DungeonManager.Instance.DungeonGenerated.RemoveListener(OnDungeonGenerated);
    }

    public void OnDungeonGenerated()
    {
        _roomInfo = GetComponent<RoomInfoGrid2D>();
        _roomInstance = _roomInfo.RoomInstance;

        foreach(var door in _roomInstance.Doors)
        {
            _corridors.Add(door.ConnectedRoomInstance.RoomTemplateInstance.GetComponent<CorridorManager>());
        }
    }

    public void OnRoomEnter()
    {
        if(IsSpawn) OpenDoors();
        else CloseDoors();
    }
    public void OnRoomComplete()
    {
        OpenDoors();
    }

    void OpenDoors()
    {
        foreach(var corridor in _corridors) corridor.OpenDoor();
    }

    void CloseDoors()
    {
        foreach(var corridor in _corridors) corridor.CloseDoor();
    }
}
