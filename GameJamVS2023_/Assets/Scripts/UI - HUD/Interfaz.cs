using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class Interfaz : MonoBehaviour
{
    [HideInInspector ]public bool HasDied;

    public GameObject[] InterfaceRunTime;
    public GameObject[] Lives;
    public GameObject[] LivesPauseMenu;

    GameObject KeyBoardBackground;
    GameObject KeyShowCaseBackground;
    GameObject InfoPanelBackground;
    public GameObject ConfirmarSalirBackground;
    bool _info;
    bool _confirm;
    public GameObject FinalIlustracion;

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
        if (SceneManager.GetActiveScene().name != "Level 1")
        {
            _currentlives = _stats.Stats.Health;
            ChangeLives(0);       
        }
        else if (SceneManager.GetActiveScene().name == "Level 1")
        {
            StartCoroutine(GetCurrentLivesDelayed());
        }      
        ShowKeysAtStart();
        //_stats.HealthChange.AddListener(ChangeLives);
        Cursor.visible = false;
        _info= false; _confirm= false;
    }

    public void FinalScreen()
    {
        FinalIlustracion.SetActive(true);
    }
    IEnumerator GetCurrentLivesDelayed()
    {
        yield return new WaitForSeconds(0.2f);
        _currentlives = _stats.Stats.Health;
    }
    private void Update()
    {
        if (!HasDied)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (Time.timeScale == 1)
                {
                    PauseMenu(0);
                }
                else if (Time.timeScale == 0)
                {
                    if (_info)
                    {
                        PauseMenu(-2);
                    }
                    else if (_confirm)
                    {
                        Debug.LogWarning("-3");
                        PauseMenu(-3);
                    }
                    else
                    {
                        PauseMenu(1);
                    }

                }
            }
            if (Input.GetKeyDown(KeyCode.Return) && _confirm)
            {
                Time.timeScale = 1;
                SceneManager.LoadScene("MainMenu");
            }
        }
        else if (HasDied)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Time.timeScale = 1;
                SceneManager.LoadScene("MainMenu");
            }
            if (Input.GetKeyDown(KeyCode.Return))
            {
                Time.timeScale = 1;
                SceneManager.LoadScene("Level 1");
            }
        }
        
    }

    

    void OnCooldownStart(Power power)
    {

        var prefab = _hUDPowers[power];
        if (!prefab) return;
        prefab.transform.GetChild(0).gameObject.SetActive(false);
        prefab.transform.GetChild(1).gameObject.SetActive(true);
    }
    void OnCooldownOver(Power power)
    {

        var prefab = _hUDPowers[power];
        if (!prefab) return;
        prefab.transform.GetChild(1).gameObject.SetActive(false);
        prefab.transform.GetChild(0).gameObject.SetActive(true);
    }

    void ShowKeysAtStart()
    {
        if (!KeyBoardBackground && !KeyShowCaseBackground && !InfoPanelBackground)
        {
            AssignKeyParents();
        }
        foreach (var power in _stats.Stats.Powers)
        {
            var uIprefab = Instantiate(power.UIPrefab, KeyBoardBackground.transform);
            _uIPowers.Add(power, uIprefab);
            var hUDprefab = Instantiate(power.HUDPrefab, KeyShowCaseBackground.transform);
            _hUDPowers.Add(power, hUDprefab);
            var infoprefab = Instantiate(power.InfoPrefab, InfoPanelBackground.transform);

            power.UsePower.AddListener(OnCooldownStart);
            power.PowerAvailable.AddListener(OnCooldownOver);

        }
    }
    public void ShowNewKey(Power power)
    {
        if (!KeyBoardBackground && !KeyShowCaseBackground && !InfoPanelBackground)
        {
            AssignKeyParents();
        }
        var uIprefab = Instantiate(power.UIPrefab, KeyBoardBackground.transform);
        _uIPowers.Add(power, uIprefab);
        var hUDprefab = Instantiate(power.HUDPrefab, KeyShowCaseBackground.transform);
        _hUDPowers.Add(power, hUDprefab);
        var infoprefab = Instantiate(power.InfoPrefab, InfoPanelBackground.transform);

        power.UsePower.AddListener(OnCooldownStart);
        power.PowerAvailable.AddListener(OnCooldownOver);
    }

    void AssignKeyParents()
    {
        KeyBoardBackground = transform.Find("Panel").Find("KeyBoardBackground").gameObject;
        KeyShowCaseBackground = transform.Find("Panel").Find("KeyShowCaseBackground").gameObject;
        InfoPanelBackground = transform.Find("Panel").Find("InfoPanelBackground").gameObject;
    }

    public void ChangeLives(int HealthChange)
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

    public void PauseMenu(int value)
    {
        if (value == 0)
        {
            Cursor.visible = true;
            Time.timeScale = 0;
            KeyBoardBackground.SetActive(true);
            foreach (var element in InterfaceRunTime) 
            { 
                element.SetActive(false);
            }
        }
        else if(value == 1)
        {
            Cursor.visible = false;
            Time.timeScale = 1;
            KeyBoardBackground.SetActive(false);
            InfoPanelBackground.SetActive(false);
            foreach (var element in InterfaceRunTime)
            {
                element.SetActive(true);
            }
        }
        else if (value == 2)
        {
            _info = true;
            Cursor.visible = true;
            InfoPanelBackground.SetActive(true);
            KeyBoardBackground.SetActive(false);
        }
        else if (value == 3)
        {
            _confirm = true;
            Cursor.visible = true;
            ConfirmarSalirBackground.SetActive(true);
            KeyBoardBackground.SetActive(false);
        }
        else if (value == -2)
        {
            _info = false;
            Cursor.visible = true;
            InfoPanelBackground.SetActive(false);
            KeyBoardBackground.SetActive(true);
        }
        else if (value == -3)
        {
            _confirm = false;
            Cursor.visible = true;
            ConfirmarSalirBackground.SetActive(false);
            KeyBoardBackground.SetActive(true);
        }
    }
}
