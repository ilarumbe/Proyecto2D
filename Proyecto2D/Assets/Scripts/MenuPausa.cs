using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPausa : MonoBehaviour
{
    public GameObject panelPausa;
    public GameObject botonPausa;

    private bool juegoPausado = false;

    void Start()
    {
        panelPausa.SetActive(false);
        botonPausa.SetActive(true);
        Time.timeScale = 1f;
    }

    public void PausarJuego()
    {
        panelPausa.SetActive(true);
        botonPausa.SetActive(false);
        Time.timeScale = 0f;
        juegoPausado = true;
    }

    public void ReanudarJuego()
    {
        panelPausa.SetActive(false);
        botonPausa.SetActive(true);
        Time.timeScale = 1f;
        juegoPausado = false;
    }

    public void SalirDelJuego()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
