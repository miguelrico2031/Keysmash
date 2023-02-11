using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanzarTecla_G : MonoBehaviour
{
    public Transform Personaje;
    public GameObject Tecla;
    public int Daño;
    public float VelocidadInicial;
    public float TiempoDeVuelo;
    public static bool TieneTeclaParaLanzar;

    private void Start()
    {
        TieneTeclaParaLanzar = false;
    }

    public void Usar()
    {
        if (TieneTeclaParaLanzar)
        {
            TeclaLanzada tecla = Tecla.GetComponent<TeclaLanzada>();
            tecla.Direccion = (Vector3)Personaje.GetComponent<Personaje>().GetFacingDirection();
            tecla.Daño = Daño;
            tecla.TiempoDeVuelo = TiempoDeVuelo;
            tecla.VelocidadInicial = VelocidadInicial;
            Instantiate(Tecla, transform.position, Quaternion.identity);
            TieneTeclaParaLanzar = false;
            Debug.Log("Lanzado");
        }
    }
}
