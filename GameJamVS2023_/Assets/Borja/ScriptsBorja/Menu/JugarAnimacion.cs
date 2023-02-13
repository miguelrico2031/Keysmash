using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class JugarAnimacion : MonoBehaviour
{
    public GameObject Panel;
    public GameObject Mano;
    public GameObject ManoDer;
    public GameObject SpriteDedo;
    public GameObject SpritePuño;
    public GameObject Teclas;
    public GameObject Teclado;
    public GameObject TecladoRoto;
    public GameObject Camara;
    public GameObject TecladoEnGrande;

    public Image FadeImage;
    public float FadeSpeed = 1.0f;

    public float[] TiempoDeEsperaEvento;

    public Transform ManoIzqPosInicial;
    public Transform TecladoPosInicial;
    public Transform ManoDerPosInicial;
    public Transform ManoDerPosFinal;

    public float KeyboardSizeSpeed;
    public float KeyboardMaxSize;
    public Transform GolpePos1;
    public float GolpeVelocidad1;
    public float GolpeAceleracion1;
    public Transform GolpePos2;
    public float GolpeVelocidad2;
    public float VelocidadRecogerTeclado;

    int _golpe = 0;
    int _final = 0;
    float _vel1;

    private void Start()
    {
        _golpe = 0;
        _vel1 = GolpeVelocidad1;
    }

    public void AnimacionDeJugar()
    {
        Panel.SetActive(false);
        
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
            Mano.GetComponent<TeclearAnimacion>().AnimacionFinal = true;
            _golpe = 5;
            StartCoroutine(EsperarRecogida());
            
            Camara.GetComponent<ScreenShake>().TriggerShake();
        }
        
        if (_final == 1)
        {
            
            Vector2 dirManoIzq = ManoIzqPosInicial.position - Mano.transform.position;

            if (dirManoIzq.magnitude >= 0.1f)
            {
                Mano.transform.Translate(dirManoIzq.normalized * VelocidadRecogerTeclado * Time.deltaTime);
                Debug.Log((dirManoIzq.normalized * VelocidadRecogerTeclado * Time.deltaTime).x);
            }
            Mano.transform.Translate(dirManoIzq.normalized * VelocidadRecogerTeclado * Time.deltaTime);

            Vector2 dir3 = ManoDerPosFinal.position - ManoDer.transform.position;
            ManoDer.transform.Translate(-dir3.normalized * VelocidadRecogerTeclado * Time.deltaTime);
            if (dir3.magnitude <= 0.05f)
            {
                _final = 2;
            }
        }
        else if (_final == 2)
        {
            
            Vector2 dirManoIzq = ManoIzqPosInicial.position - Mano.transform.position;
            if (dirManoIzq.magnitude >= 0.1f)
            {
                Mano.transform.Translate(dirManoIzq.normalized * VelocidadRecogerTeclado * Time.deltaTime);

            }

            Vector2 dir4 = ManoDerPosInicial.position - ManoDer.transform.position;
            ManoDer.transform.Translate(-dir4.normalized * VelocidadRecogerTeclado * Time.deltaTime);
            TecladoRoto.transform.Translate(dir4.normalized * VelocidadRecogerTeclado * Time.deltaTime);
            if (dir4.magnitude <= 0.05f)
            {
                _final = 3;
            }
        }else if (_final == 3)
        {
            TecladoEnGrande.SetActive(true);
            TecladoEnGrande.transform.localScale = Vector3.zero;
            _final = 4;
        }
        else if (_final == 4)
        {
            //TecladoEnGrande.transform.localScale += Vector3.one * KeyboardSizeSpeed * Time.deltaTime;
            if (TecladoEnGrande.transform.localScale.x >= KeyboardMaxSize)
            {
                
            }
            _final = 5;
        }
        else if (_final == 5)
        {
            FadeImage.color = new Color(0, 0, 0, Mathf.MoveTowards(FadeImage.color.a, 1, FadeSpeed * Time.deltaTime));
            if (FadeImage.color.a == 1)
            {
                _final = 6;
            }
        }
        else if (_final == 6)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
    IEnumerator Monologo()
    {
        yield return null;
    }
    
    IEnumerator EsperarRecogida()
    {
        if (_final == 0)
        {
            _final = -1;
            Debug.Log("recogida");
            yield return new WaitForSeconds(TiempoDeEsperaEvento[15]);
            SpriteDedo.SetActive(true);
            SpritePuño.SetActive(false);
            _final = 1;
        }
        else
        {
            yield return null;
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
