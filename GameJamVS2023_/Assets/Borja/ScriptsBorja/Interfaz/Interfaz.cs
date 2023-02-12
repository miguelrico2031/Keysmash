using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class Interfaz : MonoBehaviour
{
    public GameObject[] InterfaceRunTime;
    public GameObject[] Lives;
    public GameObject[] LivesPauseMenu;
    GameObject KeyBoardBackground;
    GameObject KeyShowCaseBackground;

    Dictionary<Power, GameObject> _hUDPowers;
    Dictionary<Power, GameObject> _uIPowers;

    StatsManager _stats;
    int _currentlives;

    private void Awake()
    {
        _hUDPowers = new Dictionary<Power, GameObject>();
        _uIPowers = new Dictionary<Power, GameObject>();

        
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
        
    }

    void OnCooldownStart(Power power)
    {
        Debug.Log(power.name + "start");
        var prefab = _hUDPowers[power];
        if (!prefab) return;
        prefab.transform.GetChild(0).gameObject.SetActive(false);
        prefab.transform.GetChild(1).gameObject.SetActive(true);
    }
    void OnCooldownOver(Power power)
    {
        Debug.Log(power.name);
        var prefab = _hUDPowers[power];
        if (!prefab) return;
        prefab.transform.GetChild(1).gameObject.SetActive(false);
        prefab.transform.GetChild(0).gameObject.SetActive(true);
    }

    void ShowKeysAtStart()
    {
        if (!KeyBoardBackground && !KeyShowCaseBackground)
        {
            AssignKeyParents();
        }
        foreach (var power in _stats.Stats.Powers)
        {
            var uIprefab = Instantiate(power.UIPrefab, KeyBoardBackground.transform);
            _uIPowers.Add(power, uIprefab);
            var hUDprefab = Instantiate(power.HUDPrefab, KeyShowCaseBackground.transform);
            _hUDPowers.Add(power, hUDprefab);

            power.UsePower.AddListener(OnCooldownStart);
            power.PowerAvailable.AddListener(OnCooldownOver);

        }
    }
    public void ShowNewKey(Power power)
    {
        if (!KeyBoardBackground && !KeyShowCaseBackground)
        {
            AssignKeyParents();
        }
        var uIprefab = Instantiate(power.UIPrefab, KeyBoardBackground.transform);
        _uIPowers.Add(power, uIprefab);
        var hUDprefab = Instantiate(power.HUDPrefab, KeyShowCaseBackground.transform);
        _hUDPowers.Add(power, hUDprefab);

        power.UsePower.AddListener(OnCooldownStart);
        power.PowerAvailable.AddListener(OnCooldownOver);
    }

    void AssignKeyParents()
    {
        KeyBoardBackground = transform.Find("Panel").Find("KeyBoardBackground").gameObject;
        KeyShowCaseBackground = transform.Find("Panel").Find("KeyShowCaseBackground").gameObject;
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
            KeyBoardBackground.SetActive(true);
            foreach (var element in InterfaceRunTime) 
            { 
                element.SetActive(false);
            }
        }
        else if(value == 1)
        {
            Time.timeScale = 1;
            KeyBoardBackground.SetActive(false);
            foreach (var element in InterfaceRunTime)
            {
                element.SetActive(true);
            }
        }
    }
}
