using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashLive : MonoBehaviour
{
    public GameObject Red;
    public GameObject Pink;
    public float FlashInterval;
    public int FlashAmount;

    int _flashCount;

    private void Start()
    {
        _flashCount = 0;
    }
    public void Flash()
    {
        StartCoroutine(FlashCoroutine());
    }
    public IEnumerator FlashCoroutine()
    {

        if (FlashAmount < _flashCount) 
        {
            _flashCount = 0;
            yield break;
        }

        Pink.SetActive(true);
        Red.SetActive(false);

        yield return new WaitForSecondsRealtime(FlashInterval);

        Pink.SetActive(false);
        Red.SetActive(true);

        yield return new WaitForSecondsRealtime(FlashInterval);

        _flashCount++;

        StartCoroutine(FlashCoroutine());
    }
}
