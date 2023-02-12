using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Melee1_Q : MonoBehaviour
{
    public int Daño;
    public float CoolDown;
    public float DuracionAtaque;
    bool _sePuedeUsar;
    bool _enAtaque;     

    private void Start()
    {
        _sePuedeUsar = true;
        _enAtaque = false;

    }

    public void Usar()
    {
        if (_sePuedeUsar && !Teclado.InAttack)
        {
            GetComponent<Collider2D>().enabled = true;
            Teclado.InAttack = true;
            _enAtaque = true;
            _sePuedeUsar = false;
            Debug.Log("Melee1");
            StartCoroutine(AttackEvents());
            StartCoroutine(EsperarCoolDown());
        }       
    }

    void OnTriggerEnter2D (Collider2D colliderEnemigo)
    {
        if (colliderEnemigo.CompareTag("Enemigo"))
        {
            if (_enAtaque)
            {
                colliderEnemigo.transform.GetComponent<Enemigo>().RecibirDaño(Daño);
                Debug.Log("Auch");
            }           
        }
    }
    IEnumerator AttackEvents()
    {
        yield return new WaitForSeconds(DuracionAtaque);
        Teclado.InAttack = false;
        GetComponent<Collider2D>().enabled = false;
    }
    IEnumerator EsperarCoolDown() 
    { 
        yield return new WaitForSeconds(CoolDown);
        _sePuedeUsar = true;
    }

}
