using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemigo : MonoBehaviour
{
    public int Vida;

    public void RecibirDa�o(int da�o)
    {
        Vida -= da�o;
        if (Vida <= 0)
        {
            Muerte();
        }
    }
    void Muerte()
    {
        Destroy(gameObject);
    }
}
