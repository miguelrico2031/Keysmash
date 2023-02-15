using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Edgar.Unity;
using UnityEngine.Events;

public class DungeonManager : DungeonGeneratorPostProcessingComponentGrid2D 
{
    public static DungeonManager Instance;

    public UnityEvent DungeonGenerated;

    void Awake()
    {
        Instance = this;
    }
    public override void Run(DungeonGeneratorLevelGrid2D level)
    {
        level.RootGameObject.transform.Find("Tilemaps").Find("Walls").gameObject.layer = LayerMask.NameToLayer("Walls");
        
        //Destroy(level.RootGameObject.transform.Find("Tilemaps").Find("Collideable").GetComponent<Rigidbody2D>());

        DungeonGenerated.Invoke();
        
    }
}
