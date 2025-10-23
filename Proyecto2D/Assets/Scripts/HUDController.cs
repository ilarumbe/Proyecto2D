using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HUDController : MonoBehaviour
{
    public GameManager gameManager;
    public TextMeshProUGUI cristales;
    void Update()
    {
        cristales.text = gameManager.Cristales.ToString();
    }
}
