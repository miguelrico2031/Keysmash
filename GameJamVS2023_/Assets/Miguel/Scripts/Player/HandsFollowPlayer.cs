using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandsFollowPlayer : MonoBehaviour
{
    public Transform Target;
    public float Speed = 4.95f;
    public float Threshold = 0.1f;

    void FixedUpdate()
    {

        float distance = Vector2.Distance(transform.position, Target.position);
        if (distance > Threshold)
        {
            Vector2 targetPosition = Target.position;
            Vector2 direction = (targetPosition - (Vector2)transform.position);
            transform.position = (Vector2)transform.position + direction * Speed * Time.fixedDeltaTime;
        }


    }
}
