using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimacionTeclasVolando : MonoBehaviour
{
    public GameObject[] Teclas;
    public GameObject Collider;
    public float Vel;
    public float TamañoVel;
    public float CapTamaño;
    public float MaxRotacion;
    public float TiempoHastaCollider;
    bool _enAnimacion;

    private void Start()
    {
        int i = 0;
        _enAnimacion = false;
        foreach (var tecla in Teclas)
        {
            tecla.GetComponent<SpriteRenderer>().enabled = false;
            i++;
        }
        //StartCoroutine(invokeTemporal());
    }

    IEnumerator invokeTemporal()
    {
        yield return new WaitForSeconds(5f);
        IniciarAnimTeclasVolando();
    }

    public void IniciarAnimTeclasVolando()
    {
        _enAnimacion = true;
        StartCoroutine(EliminarCollider());
        foreach (var tecla in Teclas)
        {
            tecla.GetComponent<SpriteRenderer>().enabled = true;
            float angulo = Random.Range(0f, 90f);
            Vector2 dir = new Vector2(Mathf.Cos(angulo), Mathf.Sin(angulo));
            tecla.GetComponent<Rigidbody2D>().velocity =  dir * Vel;
        }
    }
    IEnumerator EliminarCollider() 
    {
        yield return new WaitForSeconds(TiempoHastaCollider);
        Collider.SetActive(false);
    }

    private void FixedUpdate()
    {
        if (_enAnimacion)
        {
            foreach (var tecla in Teclas)
            {
                float x = Mathf.Clamp((tecla.transform.localScale.x + TamañoVel * Time.fixedDeltaTime), 0, CapTamaño);
                float y = Mathf.Clamp((tecla.transform.localScale.y + TamañoVel * Time.fixedDeltaTime), 0, CapTamaño);
                float z = Mathf.Clamp((tecla.transform.localScale.z + TamañoVel * Time.fixedDeltaTime), 0, CapTamaño);
                tecla.transform.localScale = new Vector3(x, y, z);
                tecla.GetComponent<Rigidbody2D>().angularVelocity = Mathf.Clamp(tecla.GetComponent<Rigidbody2D>().angularVelocity, -MaxRotacion, MaxRotacion);
                tecla.GetComponent<Rigidbody2D>().velocity = new Vector2 (Mathf.Clamp(tecla.GetComponent<Rigidbody2D>().velocity.x * Vel, -Vel, Vel), Mathf.Clamp(tecla.GetComponent<Rigidbody2D>().velocity.y * Vel, -Vel, Vel));
            }
        }
    }
}
