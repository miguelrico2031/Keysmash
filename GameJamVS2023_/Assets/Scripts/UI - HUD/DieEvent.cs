using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieEvent : MonoBehaviour
{
    GameObject _panel;
    private void Start()
    {
        _panel = transform.Find("DeathPanel").gameObject;     
    }

    public void OnDeath()
    {
        GameObject.Find("AudioManager").GetComponent<AudioManager>().PlaySound("Death");
        Time.timeScale = 0;
        GetComponent<Interfaz>().HasDied = true;
        _panel.SetActive(true);
    }

}
