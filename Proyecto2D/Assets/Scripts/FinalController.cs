using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalController : MonoBehaviour
{
    public float delay = 5f;

    void Start()
    {
        StartCoroutine(SalirTrasTiempo());
    }

    private System.Collections.IEnumerator SalirTrasTiempo()
    {
        yield return new WaitForSeconds(delay);

        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #else
                Application.Quit();
        #endif
    }
}
