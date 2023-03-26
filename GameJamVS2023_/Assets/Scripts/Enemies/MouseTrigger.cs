using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseTrigger : MonoBehaviour
{
    private Mouse _mouse;
    void Awake()
    {
        _mouse = GetComponentInParent<Mouse>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if(!_mouse.IsExploding) StartCoroutine(_mouse.StartExploding());

            if(_mouse.InExplosion) _mouse.Attack();
        }
    }
}
