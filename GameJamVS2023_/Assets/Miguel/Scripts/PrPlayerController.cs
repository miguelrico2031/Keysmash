using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrPlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 movement = new Vector2(Input.GetAxis("Vertical"), Input.GetAxis("Horizontal"));
        Debug.Log(movement);
        transform.Translate(movement * 50f * Time.deltaTime);
    }
}
