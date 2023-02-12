using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeclaPower : MonoBehaviour
{
    StatsManager _stats;

    private GameObject InterfazGO;
    public Power Power;

    void Start()
    {
        InterfazGO = GameObject.Find("Canvas");
        _stats = GameObject.FindGameObjectWithTag("Player").GetComponent<StatsManager>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (!_stats.Stats.Powers.Contains(Power))
            {
                _stats.Stats.Powers.Add(Power);
                InterfazGO.GetComponent<Interfaz>().ShowNewKey(Power);
            }      
            Destroy(gameObject);
        }
    }

}
