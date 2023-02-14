using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeclaVida : MonoBehaviour
{
    StatsManager _stats;

    private GameObject InterfazGO;

    private float _time;
    public float Amplitud = 1.0f;
    public float Frecuencia = 1.0f;

    void Start()
    {
        InterfazGO = GameObject.Find("Canvas");
        _stats = GameObject.FindGameObjectWithTag("Player").GetComponent<StatsManager>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (_stats.Stats.Health < _stats.Stats.MaxHealth)
            {
                _stats.HealPlayer(1);
                Destroy(gameObject);
            }
            
        }
    }

    void FixedUpdate()
    {
        _time += Time.deltaTime;
        float targetY = transform.position.y + Amplitud * Mathf.Sin(_time * Frecuencia * 2 * Mathf.PI);
        transform.position = Vector2.Lerp(transform.position, new Vector2(transform.position.x, targetY), Time.deltaTime);
    }
}
