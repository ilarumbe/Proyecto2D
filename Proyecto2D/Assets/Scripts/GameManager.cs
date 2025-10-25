using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instancia;

    public int muertes = 0;
    public TMP_Text muertesText;
    public int cristales = 0;
    public TMP_Text cristalesText;

    void Awake()
    {
        if (instancia == null)
        {
            instancia = this;
            DontDestroyOnLoad(gameObject);
        } else
        {
            Destroy(gameObject);
        }
    }

    public void SumarCristales(int cantidad)
    {
        cristales += cantidad;
        ActualizarUI();
    }

    public void SumarMuerte()
    {
        muertes++;
        ActualizarUI();
    }

    void Start()
    {
        ActualizarUI();
    }

    private void ActualizarUI()
    {
        cristalesText.text = cristales.ToString();
        muertesText.text = muertes.ToString();
    }

    public void CambiarEscena(string nombre)
    {
        SceneManager.LoadScene(nombre);
    }
}
