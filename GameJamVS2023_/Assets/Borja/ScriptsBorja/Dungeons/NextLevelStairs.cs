using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevelStairs : MonoBehaviour
{
    GameObject _loadingScreen;
    public string SceneName;

    private void OnTriggerEnter2D(Collider2D other)
    {      
        if (other.CompareTag("Player"))
        {
            if (_loadingScreen == null) { _loadingScreen = GameObject.Find("Canvas").transform.Find("LoadingScreen").gameObject; }
            Debug.Log(_loadingScreen);
            StartCoroutine(LoadSceneAsync(SceneName));
        }
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
}
