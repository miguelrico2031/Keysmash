using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public TextMeshProUGUI volumeText;
    public Slider volumeSlider;
    public float volumenInicial;
    public float maxRatio; // Este valor indica el maximo de volumen del slider al que llegara del volumen real. Es decir, el volumen real va de 0 a 1, pero si ponemos que maxRatio sea 0.2, el valor 0 del slider sera 0 de volumen real y el valor 1 del slider será 0.2 de volumen real.

    private void Start()
    {
        volumeSlider.value = volumenInicial;
        AudioListener.volume = volumeSlider.value * maxRatio;
        volumeText.text = (Mathf.Round(volumeSlider.value * 100f) / 100f).ToString();
    }

    public void setVolume()
    {
        AudioListener.volume = volumeSlider.value * maxRatio;
        volumeText.text = (Mathf.Round(volumeSlider.value * 100f) / 100f).ToString();
    }

    public void startGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void exitGame()
    {
        Application.Quit();
    }
}
