using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HUDController : MonoBehaviour
{
    public TextMeshProUGUI cristales;
    void Update()
    {
        if (GameManager.instancia != null && cristales != null)
        {
            cristales.text = "Cristales: " + GameManager.instancia.cristales.ToString();
        }
            
    }
}
