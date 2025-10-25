using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Inicio : MonoBehaviour
{
    public void Jugar()
    {
        SceneManager.LoadScene("nivel1");
    }

    public void Salir()
    {
        Application.Quit();
    }
}
