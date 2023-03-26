using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetPowers : MonoBehaviour
{
    public PlayerStats Stats;

    private void Start()
    {
        Stats.Powers.Clear();
    }
}
