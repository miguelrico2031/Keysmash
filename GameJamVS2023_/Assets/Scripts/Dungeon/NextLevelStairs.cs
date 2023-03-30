using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevelStairs : MonoBehaviour
{
    [SerializeField] private List<Power> _powersInFloor;

    private GameObject _warningPanel;
    public string SceneName;

    private GameObject _loadingScreen;
    private Collider2D _trigger;
    private bool _isOnWarning = false;

    private void Awake()
    {
        _trigger = GetComponent<Collider2D>();
    }

    private void Start()
    {
        _warningPanel = FindObjectOfType<Interfaz>().Warning;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        
        if (!_loadingScreen) _loadingScreen = GameObject.Find("Canvas").transform.Find("LoadingScreen").gameObject;

        PlayerStats stats = other.GetComponent<StatsManager>().Stats;

        foreach(var power in _powersInFloor)
        {
            if (stats.Powers.Contains(power)) continue;

            _trigger.enabled = false;
            _isOnWarning = true;
            _warningPanel.SetActive(true);
            return;
        }

        StartCoroutine(LoadSceneAsync(SceneName));

        
    }

    IEnumerator LoadSceneAsync(string sceneName)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        _loadingScreen.SetActive(true);
        while (!operation.isDone)
        {
            yield return null;
        }
    }

    private void Update()
    {
        if (!_isOnWarning) return;

        if(Input.GetKeyDown(KeyCode.Return))
        {
            _isOnWarning = false;
            _warningPanel.SetActive(false);
            StartCoroutine(LoadSceneAsync(SceneName));
        }
        else if(Input.GetKeyDown(KeyCode.Escape))
        {
            _isOnWarning = false;
            Invoke(nameof(ExitWarning), 0.05f);
        }
    }

    private void ExitWarning()
    {
        _warningPanel.SetActive(false);
        Invoke(nameof(ResetTrigger), 1.5f);
    }

    private void ResetTrigger() => _trigger.enabled = true;
}
