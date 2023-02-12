using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeclaLanzada : MonoBehaviour
{
    public float VelocidadInicial;
    public float TiempoDeVuelo;
    public int Daño;
    public Vector3 Direccion;
    float _aceleracion;
    float _velocidad;
    Rigidbody2D _rb2d;

    private void Start()
    {
        _velocidad = VelocidadInicial;
        _aceleracion = (-VelocidadInicial) / TiempoDeVuelo;
        _rb2d = GetComponent<Rigidbody2D>();
    }
    private void FixedUpdate()
    {
        if (_velocidad > 0) 
        {
            _velocidad += _aceleracion * Time.fixedDeltaTime;
            _rb2d.velocity = Direccion * _velocidad;

        }
        else
        {
            _velocidad = 0;
            _rb2d.velocity = Direccion * _velocidad;
        }
    }
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (_velocidad > 0.1f)
        {
            if (collider.CompareTag("Enemigo"))
            {
                collider.transform.GetComponent<Enemigo>().RecibirDaño(Daño);
                Debug.Log("Auch");
            }

        }
        else if (_velocidad < 0.1f)
        {
            if (collider.CompareTag("Personaje"))
            {
                LanzarTecla_G.TieneTeclaParaLanzar = true;
                Destroy(gameObject);
            }
        }
        
    }
}
