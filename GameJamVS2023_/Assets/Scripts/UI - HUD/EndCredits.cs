using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndCredits : MonoBehaviour
{
    public float CredsDuration;
    private void Start()
    {
        StartCoroutine(WaitForCredsToEnd());
    }
    IEnumerator WaitForCredsToEnd()
    {
        yield return new WaitForSeconds(CredsDuration);
        SceneManager.LoadScene("MainMenu");
    }
}
