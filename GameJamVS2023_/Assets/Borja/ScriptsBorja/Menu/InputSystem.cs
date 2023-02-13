using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

public class InputSystem : MonoBehaviour
{
    public GameObject[] MenuIndiceGO; // 0 es MainMenu, 1 es Opciones, 2 es Créditos
    public TextMeshProUGUI OutputText;
    public GameObject Title;

    public GameObject Personaje;
    bool _animacionEnCurso;

    public GameObject Mano;
    int _longAnterior;

    public float TiempoParpadeoBarraBaja;
    public int MaximoDeLetras;
    int _menuIndice; // 0 es MainMenu, 1 es Opciones, 2 es Créditos
    string _inputText = "";
    string _barra = "";

    private void Start()
    {
        _longAnterior = 0;
        _menuIndice = 0;
        _inputText = "";
        _barra = "";
        Cursor.visible = false;
        StartCoroutine(BarraBaja());
    }   

    void Update()
    {
        foreach (char c in Input.inputString)
        {
            if (c == '\b') // Backspace
            {
                if (_inputText.Length != 0)
                {
                    _inputText = _inputText.Substring(0, _inputText.Length - 1);
                }
            }
            else if (char.IsLetter(c) && _inputText.Length <= MaximoDeLetras)
            {
                if (!_animacionEnCurso)
                {
                    _inputText += c;
                }
                
            }
        }

        EnseñarInput();

        if (Input.GetKeyDown(KeyCode.Return))
        {
            LeerInput();
            Mano.GetComponent<TeclearAnimacion>().AnimacionTeclear(0);
        }

        if (!Mano.GetComponent<TeclearAnimacion>().Ida)
        {
            if (_longAnterior > _inputText.Length)
            {
                Mano.GetComponent<TeclearAnimacion>().AnimacionTeclear(1);
            }
            else if (_longAnterior < _inputText.Length)
            {
                Mano.GetComponent<TeclearAnimacion>().AnimacionTeclear((Random.Range(2, 10)));
                
            }
        }
        _longAnterior = _inputText.Length;


        
    }

    IEnumerator BarraBaja()
    {
        if (_barra == "_")
        {
            _barra = "";
        }
        else if (_barra == "")
        {
            _barra = "_";
        }

        yield return new WaitForSeconds(TiempoParpadeoBarraBaja);
        StartCoroutine(BarraBaja());
    }


    void EnseñarInput()
    {
        OutputText.text = _inputText + _barra;
    }

    void LeerInput()
    {
        if (_inputText == "config" || _inputText == "Config")
        {
            if (_menuIndice == 0)
            {
                _menuIndice = 1;
                MenuIndiceGO[0].SetActive(false);
                Title.SetActive(false);
                MenuIndiceGO[1].SetActive(true);
            }
            
        }
        else if (_inputText == "jugar" || _inputText == "Jugar")
        {
            if (_menuIndice == 0)
            {
                Personaje.GetComponent<JugarAnimacion>().AnimacionDeJugar();
                _animacionEnCurso = true;
            }
            
        }
        else if (_inputText == "creds" || _inputText == "Creds" || _inputText == "créds" || _inputText == "Créds")
        {
            if (_menuIndice == 0)
            {
                _menuIndice = 2;
                MenuIndiceGO[0].SetActive(false);
                Title.SetActive(false);
                MenuIndiceGO[2].SetActive(true);
            }
            
        }
        else if (_inputText == "salir" || _inputText == "Salir")
        {
            if (_menuIndice == 0)
            {
                Debug.Log("Salir...");
                Application.Quit();
            }
            
        }
        else if (_inputText == "volver" || _inputText == "Volver")
        {
            if (_menuIndice != 0)
            {
                MenuIndiceGO[2].SetActive(false);
                MenuIndiceGO[1].SetActive(false);
                MenuIndiceGO[0].SetActive(true);
                Title.SetActive(true);
                _menuIndice = 0;
                Debug.Log("Atrás...");
            }
        }
        else
        {
            Debug.Log("ERROR");
        }
        _inputText = "";
    }
}

