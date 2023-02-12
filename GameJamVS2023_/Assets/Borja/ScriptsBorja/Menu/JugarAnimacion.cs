using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class JugarAnimacion : MonoBehaviour
{
    public GameObject Mano;
    public GameObject SpriteDedo;
    public GameObject SpritePuño;
    public GameObject Teclas;
    public GameObject Teclado;
    public GameObject TecladoRoto;

    public float[] TiempoDeEsperaEvento;

    public Transform GolpePos1;
    public float GolpeVelocidad1;
    public float GolpeAceleracion1;
    public Transform GolpePos2;
    public float GolpeVelocidad2;

    int _golpe = 0;
    float _vel1;

    private void Start()
    {
        _golpe = 0;
        _vel1 = GolpeVelocidad1;
    }

    public void AnimacionDeJugar()
    {
        StartCoroutine(Eventos());  
    }

    private void Update()
    {
        if (_golpe == 1)
        {
            Vector3 dir1 = GolpePos1.position - Mano.transform.position;
            Mano.transform.Translate(dir1.normalized * _vel1 * Time.deltaTime);
            _vel1 += GolpeAceleracion1 * Time.deltaTime;
            if (dir1.magnitude <= 0.05f)
            {
                _golpe = 2;
            }
        }
        else if (_golpe == 2)
        {
            StartCoroutine(EsperarGolpe());
        }
        else if (_golpe == 3)
        {
            Vector3 dir2 = GolpePos2.position - Mano.transform.position;
            Mano.transform.Translate(dir2.normalized * GolpeVelocidad2 * Time.deltaTime);
            if (dir2.magnitude <= 0.2f)
            {
                Mano.transform.position = GolpePos2.transform.position;
                _golpe = 4;
            }

        }
        else if (_golpe == 4)
        {
            Teclas.GetComponent<AnimacionTeclasVolando>().IniciarAnimTeclasVolando();
            Teclado.SetActive(false);
            TecladoRoto.SetActive(true);
            _golpe= 5;
        }

    }
    IEnumerator EsperarGolpe()
    {
        yield return new WaitForSeconds(TiempoDeEsperaEvento[14]);
        _golpe = 3;
    }
    IEnumerator Eventos()
    {
        yield return new WaitForSeconds(TiempoDeEsperaEvento[0]);
        Mano.GetComponent<TeclearAnimacion>().AnimacionTeclear(0);
        yield return new WaitForSeconds(TiempoDeEsperaEvento[1]);
        Mano.GetComponent<TeclearAnimacion>().AnimacionTeclear(0);
        yield return new WaitForSeconds(TiempoDeEsperaEvento[2]);
        Mano.GetComponent<TeclearAnimacion>().AnimacionTeclear(0);
        yield return new WaitForSeconds(TiempoDeEsperaEvento[3]);
        Mano.GetComponent<TeclearAnimacion>().AnimacionTeclear(0);
        yield return new WaitForSeconds(TiempoDeEsperaEvento[4]);
        Mano.GetComponent<TeclearAnimacion>().AnimacionTeclear(0);
        yield return new WaitForSeconds(TiempoDeEsperaEvento[5]);
        Mano.GetComponent<TeclearAnimacion>().AnimacionTeclear(0);
        yield return new WaitForSeconds(TiempoDeEsperaEvento[6]);
        Mano.GetComponent<TeclearAnimacion>().AnimacionTeclear(0);
        yield return new WaitForSeconds(TiempoDeEsperaEvento[7]);
        Mano.GetComponent<TeclearAnimacion>().AnimacionTeclear(0);
        yield return new WaitForSeconds(TiempoDeEsperaEvento[8]);
        Mano.GetComponent<TeclearAnimacion>().AnimacionTeclear(0);
        yield return new WaitForSeconds(TiempoDeEsperaEvento[9]);
        Mano.GetComponent<TeclearAnimacion>().AnimacionTeclear(0);
        yield return new WaitForSeconds(TiempoDeEsperaEvento[10]);
        Mano.GetComponent<TeclearAnimacion>().AnimacionTeclear(0);
        yield return new WaitForSeconds(TiempoDeEsperaEvento[11]);
        Mano.GetComponent<TeclearAnimacion>().AnimacionTeclear(0);
        yield return new WaitForSeconds(TiempoDeEsperaEvento[12]);
        SpriteDedo.SetActive(false);
        SpritePuño.SetActive(true);
        yield return new WaitForSeconds(TiempoDeEsperaEvento[13]);
        _golpe = 1;
    }
}
