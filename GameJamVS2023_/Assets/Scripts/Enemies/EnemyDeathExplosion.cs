using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeathExplosion : MonoBehaviour
{
    public void OnExplosionEnd()
    {
        Destroy(gameObject);
    }
}
