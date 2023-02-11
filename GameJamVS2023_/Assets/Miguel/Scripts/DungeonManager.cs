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
        DungeonGenerated.Invoke();
    }
}
