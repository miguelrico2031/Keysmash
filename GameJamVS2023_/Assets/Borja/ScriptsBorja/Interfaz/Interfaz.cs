using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interfaz : MonoBehaviour
{
    public GameObject KeyboardPauseMenu;
    public GameObject[] InterfaceRunTime;
    public GameObject[] Lives;
    public GameObject[] LivesPauseMenu;
    public GameObject KeyBoardBackground;
    public GameObject KeyShowCaseBackground;

    Dictionary<GameObject, Power> _hUDPowers;
    Dictionary<GameObject, Power> _uIPowers;

    StatsManager _stats;
    int _currentlives;

    private void Awake()
    {
        _hUDPowers = new Dictionary<GameObject, Power>();
        _uIPowers = new Dictionary<GameObject, Power>();
    }
    private void Start()
    {
        _stats = GameObject.FindGameObjectWithTag("Player").GetComponent<StatsManager>();
        _currentlives = _stats.Stats.Health;
        ChangeLives(0);
        ShowKeysAtStart();
        _stats.HealthChange.AddListener(ChangeLives);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Time.timeScale == 1)
            {
                PauseMenu(0);
            }else if (Time.timeScale == 0)
            {
                PauseMenu(1);
            }
        }
        ShowCooldown();
    }

    void ShowCooldown()
    {
        foreach (var prefab in _hUDPowers)
        {
            if (!prefab.Value.CoolDownOver)
            {
                prefab.Key.transform.GetChild(0).gameObject.SetActive(false);
                prefab.Key.transform.GetChild(1).gameObject.SetActive(true);
            }else if (prefab.Value.CoolDownOver)
            {
                prefab.Key.transform.GetChild(1).gameObject.SetActive(false);
                prefab.Key.transform.GetChild(0).gameObject.SetActive(true);
            }
        }
    }

    void ShowKeysAtStart()
    {
        foreach (var power in _stats.Stats.Powers)
        {
            var uIprefab = Instantiate(power.UIPrefab, KeyBoardBackground.transform);
            _uIPowers.Add(uIprefab, power);
            var hUDprefab = Instantiate(power.HUDPrefab, KeyShowCaseBackground.transform);
            _hUDPowers.Add(hUDprefab, power);
        }   
    }
    void ShowNewKey(Power power)
    {
        var uIprefab = Instantiate(power.UIPrefab, KeyBoardBackground.transform);
        _uIPowers.Add(uIprefab, power);
        var hUDprefab = Instantiate(power.HUDPrefab, KeyShowCaseBackground.transform);
        _hUDPowers.Add(hUDprefab, power);
    }

    void ChangeLives(int HealthChange)
    {        
        _currentlives += HealthChange;
        for (int i = 0; i < Lives.Length; i++)
        {  
            if (_currentlives >= i + 1)
            {
                Lives[i].SetActive(true);
                LivesPauseMenu[i].SetActive(true);
                if (HealthChange < 0)
                {
                    Lives[i].GetComponent<FlashLive>().Flash();
                }
            }
            else if (_currentlives < i + 1)
            {
                Lives[i].SetActive(false);
                LivesPauseMenu[i].SetActive(false);
            }
        }
        
    }

    void PauseMenu(int value)
    {
        if (value == 0)
        {
            Time.timeScale = 0;
            KeyboardPauseMenu.SetActive(true);
            foreach (var element in InterfaceRunTime) 
            { 
                element.SetActive(false);
            }
        }
        else if(value == 1)
        {
            Time.timeScale = 1;
            KeyboardPauseMenu.SetActive(false);
            foreach (var element in InterfaceRunTime)
            {
                element.SetActive(true);
            }
        }
    }
}
