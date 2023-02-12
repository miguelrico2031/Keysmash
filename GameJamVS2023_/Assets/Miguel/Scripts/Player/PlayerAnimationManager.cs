using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationManager : MonoBehaviour
{

    private PlayerMovement _playerMovement;
    private Animator _feetAnimator, _headAnimator;
    private Rigidbody2D _rb;

    void Awake()
    {
        _playerMovement = GetComponent<PlayerMovement>();
        _rb = GetComponent<Rigidbody2D>();
        
        for (int i = 0; i < transform.childCount; i++)
        {
            var child = transform.GetChild(i);
            if (child.CompareTag("Feet") && !_feetAnimator)
            {
                _feetAnimator = child.GetComponent<Animator>();
                
            }
            else if (child.CompareTag("Head") && !_headAnimator)
            {
                _headAnimator = child.GetComponent<Animator>();
                
            }

            if(_headAnimator && _feetAnimator) break;
        }

    }

    void FixedUpdate()
    {
        _feetAnimator.SetFloat("Speed", _rb.velocity.magnitude);
        _feetAnimator.SetFloat("Horizontal", _playerMovement.RawDirection.x);
        _feetAnimator.SetFloat("Vertical", _playerMovement.RawDirection.y);

        _headAnimator.SetFloat("Speed", _rb.velocity.magnitude);
        _headAnimator.SetFloat("Horizontal", _playerMovement.RawDirection.x);
        _headAnimator.SetFloat("Vertical", _playerMovement.RawDirection.y);
    }
}
