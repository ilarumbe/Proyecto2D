using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private int cristales;
    public int Cristales { get { return cristales; } }
    public void SumarCristales(int cristalesASumar)
    {
        cristales += cristalesASumar;
        Debug.Log("Cristales: " + cristales);
    }
}
