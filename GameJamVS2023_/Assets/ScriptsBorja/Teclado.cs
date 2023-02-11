using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teclado : MonoBehaviour
{
    public Transform Mano;
    public static bool InAttack;

    void Start()
    {
        GetComponent<Collider2D>().enabled = false;
    }
    private void Update()
    {
        if (!InAttack)
        {
            transform.position = Mano.position;
        }
        
    }

}
