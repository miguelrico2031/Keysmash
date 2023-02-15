using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

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

    public string SceneToLoadName;
    public GameObject LoadingScreen;
    public TextMeshProUGUI LoadingProgressText;

    public float VignetasVel;
    public Image Vigneta1;
    public TextMeshProUGUI Vigneta1Text;
    public float Vigneta1Time;
    public Image Vigneta2;
    public TextMeshProUGUI Vigneta2Text;
    public float Vigneta2Time;
    public Image Vigneta3;
    public TextMeshProUGUI Vigneta3Text;
    public float Vigneta3Time;
    bool _vignetasCoroutine;
    public float VelocidadVolumen;
    bool _bajarVolumen;

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
        _vignetasCoroutine= false;
        _bajarVolumen = false;
    }

    public void AnimacionDeJugar()
    {
        Panel.SetActive(false);
        
        StartCoroutine(Eventos());
    }

    private void Update()
    {
        

        if (_bajarVolumen)
        {
            Camara.GetComponent<AudioSource>().volume -= VelocidadVolumen * Time.deltaTime;
        }
        
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
            }
            Mano.transform.Translate(dirManoIzq.normalized * VelocidadRecogerTeclado * Time.deltaTime);

            Vector2 dir3 = ManoDerPosFinal.position - ManoDer.transform.position;
            ManoDer.transform.Translate(-dir3.normalized * VelocidadRecogerTeclado * Time.deltaTime);
            if (dir3.magnitude <= 0.1f)
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
            if (dir4.magnitude <= 0.1f)
            {
                _final = 5;
            }
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
            if (!_vignetasCoroutine) { StartCoroutine(Vignetas(Vigneta1Time)); }
            
            Vigneta1.color = new Color(1, 1, 1, Mathf.MoveTowards(Vigneta1.color.a, 1, VignetasVel * Time.deltaTime));
            Vigneta1Text.color = new Color(1, 1, 1, Mathf.MoveTowards(Vigneta1.color.a, 1, VignetasVel * 2 * Time.deltaTime));
            if (Vigneta1.color.a == 1)
            {
                if (!_vignetasCoroutine) { StartCoroutine(Vignetas(Vigneta1Time)); }
            }
        }
        else if (_final == 7)
        {
            Vigneta2.color = new Color(1, 1, 1, Mathf.MoveTowards(Vigneta2.color.a, 1, VignetasVel * Time.deltaTime));
            Vigneta2Text.color = new Color(1, 1, 1, Mathf.MoveTowards(Vigneta2.color.a, 1, VignetasVel * 2 * Time.deltaTime));
            Vigneta1Text.color = new Color(0, 0, 0 , 0);
            if (Vigneta2.color.a == 1)
            {
                if (!_vignetasCoroutine) { StartCoroutine(Vignetas(Vigneta2Time)); }
            }
        }
        else if (_final == 8)
        {
            Vigneta3Text.color = new Color(1, 1, 1, Mathf.MoveTowards(Vigneta3Text.color.a, 1, VignetasVel * 2 * Time.deltaTime));
            Vigneta2Text.color = new Color(0, 0, 0, 0);
            if (Vigneta3Text.color.a == 1)
            {
                if (!_vignetasCoroutine) { StartCoroutine(Vignetas(Vigneta3Time / 1.5f)); }
            }
        }
        else if (_final == 9)
        {
            Vigneta3.color = new Color(1, 1, 1, Mathf.MoveTowards(Vigneta3.color.a, 1, VignetasVel / 2 * Time.deltaTime));
            Vigneta3Text.color = new Color(0, 0, 0, 0);
            if (Vigneta3.color.a == 1)
            {
                if (!_vignetasCoroutine) { StartCoroutine(Vignetas(Vigneta3Time)); }
            }
        }
        else if (_final == 10)
        {

            StartLoading();
        }
    }

    public void StartLoading()
    {
        StartCoroutine(LoadSceneAsync(SceneToLoadName));
    }

    IEnumerator LoadSceneAsync(string sceneName)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        LoadingScreen.SetActive(true);
        while(!operation.isDone)
        {
            yield return null;
        }
    }

    IEnumerator Vignetas(float time)
    {
        if (_vignetasCoroutine) { yield break; }
        _vignetasCoroutine = true;
        yield return new WaitForSeconds(time);
        _final++;
        _vignetasCoroutine = false;
    }
    
    IEnumerator EsperarRecogida()
    {
        if (_final == 0)
        {
            _final = -1;
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
        _bajarVolumen = true;
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
