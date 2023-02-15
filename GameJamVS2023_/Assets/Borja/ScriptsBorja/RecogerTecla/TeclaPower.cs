using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeclaPower : MonoBehaviour
{
    StatsManager _stats;

    private GameObject _interfazGO;
    private GameObject _soundManager;
    public Power Power;

    void Start()
    {
        _interfazGO = GameObject.Find("Canvas");
        _soundManager = GameObject.Find("AudioManager");
        _stats = GameObject.FindGameObjectWithTag("Player").GetComponent<StatsManager>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (!_stats.Stats.Powers.Contains(Power))
            {
                _stats.Stats.Powers.Add(Power);
                Power.OnStart();
                _interfazGO.GetComponent<Interfaz>().ShowNewKey(Power);
                _soundManager.GetComponent<AudioManager>().PlaySound("NewPower");
            }      
            Destroy(gameObject);
        }
    }

}
