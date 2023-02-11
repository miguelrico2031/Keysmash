using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorridorManager : MonoBehaviour
{
    private GameObject _door;
    // Start is called before the first frame update
    public bool Open = false;

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

    // Update is called once per frame
    void OnDungeonGenerated()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            var child = transform.GetChild(i);
            if (child.CompareTag("Door"))
            {
                _door = child.gameObject;
                break;
            }
        }

    }

    public void OpenDoor()
    {
        _door.SetActive(false);
        Open = true;
    }

    public void CloseDoor()
    {
        _door.SetActive(true);
        Open = false;
    }
}
