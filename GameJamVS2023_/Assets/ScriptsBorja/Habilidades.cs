using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Habilidades : MonoBehaviour
{
    public Transform Teclado;
    public float RadioTeclado;

    public void Dash()
    {

    }
    public void MeleeBasico()
    {
        Collider2D[] objetos = Physics2D.OverlapCircleAll(Teclado.position, RadioTeclado);

        foreach (Collider2D colisionador in objetos)
        {
            if (colisionador.CompareTag("Enemigo"))
            {
                colisionador.transform.GetComponent<Enemigo>().RecibirDaño(1);
            }
        }
    }
    public void RangoBoomerang()
    {

    }
    public void Escudo()
    {

    }

    private void Update()
    {
        
    }
}
