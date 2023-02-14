using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lizard : Enemy
{
    [SerializeField] private float _fireballSpeed, _fireballRotationSpeed, _hideDuration, _hideRadius;
    [SerializeField] private FireBall _fireball;

    private Bounds _roomBounds;

    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
    }

    public override void Attack()
    {
        
    }

    void SelectAppearPoint()
    {
        Vector2 position = new Vector2(Random.Range(_roomBounds.min.x, _roomBounds.max.x), Random.Range(_roomBounds.min.y, _roomBounds.max.y));
        transform.position = position;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Room"))
        {
            _roomBounds = other.bounds;
        }
    }

}
