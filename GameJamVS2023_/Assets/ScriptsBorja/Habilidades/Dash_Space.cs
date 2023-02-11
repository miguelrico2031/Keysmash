using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class Dash_Space : MonoBehaviour
{
    public Transform Personaje;
    public float VelocidadInicial;
    public float DuracionDash;
    public float CoolDown;
    public static bool EnDash;
    bool _sePuedeUsar;
    float _velocidad;
    float _aceleracion;
    Rigidbody2D _rb2dPersonaje;
    Vector3 _direccion;

    private void Start()
    {
        _sePuedeUsar  = true;
    }

    public void Usar()
    {
        if (_sePuedeUsar && !Teclado.InAttack) 
        {
            Debug.Log("Dash");
            _sePuedeUsar = false;
            Dash();
            StartCoroutine(EsperarCoolDown());
        }
    }

    void Dash()
    {
        Debug.Log("omh hai");
        _velocidad = VelocidadInicial;
        _aceleracion = (-VelocidadInicial) / DuracionDash;
        _rb2dPersonaje = Personaje.GetComponent<Rigidbody2D>();
        _direccion = (Vector3)Personaje.GetComponent<Personaje>().GetFacingDirection();
        EnDash = true;
    }
    private void FixedUpdate()
    {
        if (EnDash)
        {
            _velocidad += _aceleracion * Time.fixedDeltaTime;
            _rb2dPersonaje.velocity = _direccion * _velocidad;
            if (_velocidad <= 0)
            {
                _velocidad = 0;
                _rb2dPersonaje.velocity = _direccion * _velocidad;
                EnDash = false;
            }
        }
        
    }

    IEnumerator EsperarCoolDown()
    {
        yield return new WaitForSeconds(CoolDown);
        _sePuedeUsar = true;
    }
}
