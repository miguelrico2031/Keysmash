using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Keyboard : MonoBehaviour
{
    public static Keyboard Instance;

    public bool Attacking = false;

    [SerializeField] private Transform _hand;

    private BoxCollider2D _collider;
    void Awake()
    {
        Instance = this;
        _collider = GetComponent<BoxCollider2D>();
    }
    private void Start()
    {
        _collider.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(!Attacking) transform.position = _hand.position;
    }
}
