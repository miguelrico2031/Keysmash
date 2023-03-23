using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUp : MonoBehaviour
{
    void Start()
    {
        Invoke("Hide", 10f);
    }

    void Hide()
    {
        gameObject.SetActive(false);
    }

}
