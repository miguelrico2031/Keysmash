using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateHands : MonoBehaviour
{
    public bool IsBlocked = false;

    [SerializeField] private float _rotationSpeed;
    private PlayerMovement _playerMovement;
    private Transform _leftHand, _rightHand;
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
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(IsBlocked) return;

        float angle = Mathf.Atan2(_playerMovement.Direction.y, _playerMovement.Direction.x) * Mathf.Rad2Deg;
        
        Quaternion rotation = Quaternion.AngleAxis(angle + 90f, Vector3.forward);
        Quaternion rotation2 = Quaternion.AngleAxis(angle, Vector3.forward);
        Quaternion rotation3 = Quaternion.AngleAxis(-angle, Vector3.forward);
        Quaternion smoothRotation =  Quaternion.RotateTowards(transform.rotation, rotation, _rotationSpeed * Time.fixedDeltaTime);
        transform.rotation = smoothRotation;
        _leftHand.rotation = smoothRotation;
        //_leftHand.Rotate(Vector3.forward, -90);
        _rightHand.rotation = smoothRotation;
        //_rightHand.Rotate(Vector3.forward, 90);
    }
}
