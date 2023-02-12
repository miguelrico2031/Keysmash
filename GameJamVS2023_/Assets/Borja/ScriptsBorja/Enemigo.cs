using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemigo : MonoBehaviour
{
    public int Vida;

    public void RecibirDaño(int daño)
    {
        Vida -= daño;
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
