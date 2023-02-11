using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Boomerang_R : MonoBehaviour
{
    public int Daño;
    public float CoolDown;
    public float TiempoAntesDeVuelta;
    public float VelocidadInicial;
    public Transform Personaje;
    public Transform Mano;
    //public Transform _posObjetivo;
    Vector3 _direccion;
    float _velocidad;
    float _aceleracion;
    bool _sePuedeUsar;
    bool _enAtaque;
    bool _ida;
    bool _vuelta;




    private void Start()
    {
        _sePuedeUsar = true;
        _enAtaque = false;
        _ida = false;
        _vuelta = false;
        
    }

    public void Usar()
    {
        if (_sePuedeUsar && !Teclado.InAttack)
        {
            GetComponent<Collider2D>().enabled = true;
            Teclado.InAttack = true;
            _enAtaque = true;
            _sePuedeUsar = false;
            Debug.Log("Boomerang");
            
            StartCoroutine(AttackEvents());
            StartCoroutine(EsperarCoolDown());
        }
    }

    void OnTriggerEnter2D(Collider2D colliderEnemigo)
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
        _velocidad = VelocidadInicial;
        _direccion = (Vector3)Personaje.GetComponent<Personaje>().GetFacingDirection();
        _aceleracion = (-VelocidadInicial) / TiempoAntesDeVuelta;
        _ida = true;
        yield return new WaitForSeconds(TiempoAntesDeVuelta);
        _ida = false;
        _vuelta = true;       
    }
    void FixedUpdate  ()
    {
        if (_ida)
        {
            _velocidad += _aceleracion * Time.fixedDeltaTime;            
            transform.Translate(_direccion * _velocidad * Time.fixedDeltaTime, Space.World);
        }
        else if (_vuelta)
        {
            _velocidad = Mathf.Clamp((_velocidad - _aceleracion * Time.fixedDeltaTime), -VelocidadInicial, VelocidadInicial);
            Vector2 dir = Mano.position - transform.position;
            transform.Translate((Vector3)dir.normalized * _velocidad * Time.fixedDeltaTime);
            if (dir.magnitude <= 0.15f)
            {
                RecogerBoomerang();
            }
        }
        
    }
    void RecogerBoomerang()
    {
        Debug.Log("VuelveBoomerang");
        _vuelta = false;
        _enAtaque = false;
        Teclado.InAttack = false;
        GetComponent<Collider2D>().enabled = false;
    }

    IEnumerator EsperarCoolDown()
    {
        yield return new WaitForSeconds(CoolDown);
        _sePuedeUsar = true;
    }

}
