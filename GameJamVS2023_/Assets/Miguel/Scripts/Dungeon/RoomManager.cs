using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Edgar.Unity;

public abstract class RoomManager : MonoBehaviour 
{
    // Start is called before the first frame update
    public bool IsSpawned;


    protected RoomInfoGrid2D _roomInfo;
    protected RoomInstanceGrid2D _roomInstance;
    protected List<CorridorManager> _corridors;


    public virtual void Awake()
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

    public virtual void Start()
    {
        DungeonManager.Instance.DungeonGenerated.AddListener(OnDungeonGenerated);
    }

    public virtual void OnDungeonGenerated()
    {
        IsSpawned = true;
        _roomInfo = GetComponent<RoomInfoGrid2D>();
        _roomInstance = _roomInfo.RoomInstance;

        foreach(var door in _roomInstance.Doors)
        {
            _corridors.Add(door.ConnectedRoomInstance.RoomTemplateInstance.GetComponent<CorridorManager>());
        }
    }

    public void OpenDoors()
    {
        foreach(var corridor in _corridors) corridor.OpenDoor();
    }

    public void CloseDoors()
    {
        foreach(var corridor in _corridors) corridor.CloseDoor();
    }
}
