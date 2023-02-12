using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleMano : MonoBehaviour
{
    
    private float _time;
    public float Amplitud = 1.0f;
    public float Frecuencia = 1.0f;
    void FixedUpdate()
    {
        _time += Time.deltaTime;
        float targetY = transform.position.y + Amplitud * Mathf.Sin(_time * Frecuencia * 2 * Mathf.PI);
        transform.position = Vector2.Lerp(transform.position, new Vector2(transform.position.x, targetY), Time.deltaTime);
    }
    
}
