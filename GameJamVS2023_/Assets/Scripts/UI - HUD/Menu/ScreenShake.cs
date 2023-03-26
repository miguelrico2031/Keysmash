using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShake : MonoBehaviour
{
    public float shakeDuration = 0.5f;
    public float shakeMagnitude = 1.0f;

    private Vector3 originalPos;
    private float shakeElapsedTime = 0.0f;

    private void Start()
    {
        originalPos = transform.localPosition;
    }

    private void Update()
    {
        if (shakeElapsedTime > 0)
        {
            transform.localPosition = originalPos + Random.insideUnitSphere * shakeMagnitude;
            shakeElapsedTime -= Time.deltaTime;
        }
        else
        {
            shakeElapsedTime = 0f;
            transform.localPosition = originalPos;
        }
    }

    public void TriggerShake()
    {
        shakeElapsedTime = shakeDuration;
    }
}
