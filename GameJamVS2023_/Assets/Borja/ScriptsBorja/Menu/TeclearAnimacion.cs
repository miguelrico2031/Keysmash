using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TeclearAnimacion : MonoBehaviour
{
    public bool AnimacionFinal;
    public Transform ManoPosInicial;
    public Transform Teclado;
    public Transform OtraMano;
    public Transform TecladoPosInicial;
    public Transform OtraManoPosInicial;
    public Transform[] PuntosDeAnimacion;
    public float VelocidadIda;
    public float VelocidadVuelta;
    public float VelocidadTeclado;
    public float TiempoAnimacionTeclado;
    Transform _objetivo;
    public bool Ida;
    int _tecladoSeMueve;
    bool _enAnimacion;

    void Start()
    {
        Ida = false;
        _enAnimacion = false;
        AnimacionFinal = false;
    }

    public void AnimacionTeclear(int punto)
    {
        _objetivo = PuntosDeAnimacion[punto];
        Ida = true;
        _enAnimacion = true;
    }
    IEnumerator AnimacionTeclado()
    {
        _tecladoSeMueve = 1;
        yield return new WaitForSeconds(TiempoAnimacionTeclado/2);
        _tecladoSeMueve = 2;
        yield return new WaitForSeconds(TiempoAnimacionTeclado/2);
        _tecladoSeMueve = 0;
    }


    void Update()
    {
        if (!AnimacionFinal)
        {
            if (_enAnimacion)
            {
                if (Ida)
                {
                    Vector3 direccion = _objetivo.position - transform.position;
                    transform.Translate(direccion.normalized * VelocidadIda * Time.deltaTime);
                    if (direccion.magnitude <= 0.1f)
                    {
                        Ida = false;
                        if (_tecladoSeMueve == 0)
                        {
                            StartCoroutine(AnimacionTeclado());
                        }

                    }
                }
                else if (!Ida)
                {
                    Vector3 dir = ManoPosInicial.position - transform.position;
                    transform.Translate(dir.normalized * VelocidadVuelta * Time.deltaTime);
                    if (dir.magnitude <= 0.1f)
                    {
                        _enAnimacion = false;
                    }
                }
            }
            if (_tecladoSeMueve == 1)
            {
                Teclado.Translate(Vector3.down * VelocidadTeclado * Time.deltaTime);
                OtraMano.Translate(Vector3.up * VelocidadTeclado * Time.deltaTime);
            }
            else if (_tecladoSeMueve == 2)
            {
                Vector3 dirT = TecladoPosInicial.position - Teclado.position;
                Vector3 dirM = OtraManoPosInicial.position - OtraMano.position;
                Teclado.Translate(dirT * VelocidadTeclado * Time.deltaTime);
                OtraMano.Translate(dirM * VelocidadTeclado * Time.deltaTime);
            }
            else if (_tecladoSeMueve == 0)
            {
                if (!AnimacionFinal)
                {
                    Teclado.position = TecladoPosInicial.position;
                    OtraMano.position = OtraManoPosInicial.position;
                }

            }
        }
        
    }
}
