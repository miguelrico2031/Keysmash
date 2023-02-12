using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateHands : MonoBehaviour
{
    public bool IsBlocked = false;

    [SerializeField] private float _rotationSpeed;
    private PlayerMovement _playerMovement;
    private Transform _leftHand, _rightHand;
    //private HandsFollowPlayer _leftHandFollowPlayer, _rightHandFollowPlayer;

    private Vector2 _lastDirection;
    void Awake()
    {
        _playerMovement = GetComponentInParent<PlayerMovement>();
        Transform playerParent = transform.parent.parent;
        for (int i = 0; i < playerParent.childCount; i++)
        {
            var child = playerParent.GetChild(i);
            if (child.CompareTag("Hand"))
            {
                if(child.GetComponent<HandsFollowPlayer>().isRightHand) _rightHand = child;
                else _leftHand = child;
            }
        }

        //_leftHandFollowPlayer = _leftHand.GetComponent<HandsFollowPlayer>();
        //_rightHandFollowPlayer = _rightHand.GetComponent<HandsFollowPlayer>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(IsBlocked) return;

        if(_lastDirection != _playerMovement.Direction && _lastDirection + _playerMovement.Direction == Vector2.zero)
        {
            transform.Rotate(Vector3.forward, 180f);
            _leftHand.Rotate(Vector3.forward, 180f);
            _rightHand.Rotate(Vector3.forward, 180f);
            //_leftHandFollowPlayer.SetHandInPosition();
            //_rightHandFollowPlayer.SetHandInPosition();
        }

        float angle = Mathf.Atan2(_playerMovement.Direction.y, _playerMovement.Direction.x) * Mathf.Rad2Deg;
        
        Quaternion rotation = Quaternion.AngleAxis(angle + 90f, Vector3.forward);
        Quaternion smoothRotation =  Quaternion.RotateTowards(transform.rotation, rotation, _rotationSpeed * Time.fixedDeltaTime);
        transform.rotation = smoothRotation;
        _leftHand.rotation = smoothRotation;
        _leftHand.Rotate(Vector3.forward, 90);
        _rightHand.rotation = smoothRotation;
        _rightHand.Rotate(Vector3.forward, -90);

        _lastDirection = _playerMovement.Direction;
    }
}
