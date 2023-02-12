using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Edgar.Unity;

public class RoomManager : MonoBehaviour 
{
    // Start is called before the first frame update
    public bool IsSpawned;

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
        if(!DungeonManager.Instance) return;
        DungeonManager.Instance.DungeonGenerated.AddListener(OnDungeonGenerated);
    }

    private void OnDisable()
    {
        DungeonManager.Instance.DungeonGenerated.RemoveListener(OnDungeonGenerated);
    }

    private void Start()
    {
        DungeonManager.Instance.DungeonGenerated.AddListener(OnDungeonGenerated);
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
        if(IsSpawned) OpenDoors();
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