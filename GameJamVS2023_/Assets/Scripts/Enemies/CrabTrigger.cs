using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrabTrigger : MonoBehaviour
{
    private Crab _crab;

    private void Start()
    {
        _crab = GetComponentInParent<Crab>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        //yo q se igual luego sirve
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if(other.gameObject.layer != LayerMask.NameToLayer("Player")) return;

        if(_crab.State == CrabState.Mimic || _crab.State == CrabState.Chase) _crab.StartAttackProcess();

        else if(_crab.State == CrabState.Attack && other.gameObject.CompareTag("Player")) _crab.Attack();
    }
}
