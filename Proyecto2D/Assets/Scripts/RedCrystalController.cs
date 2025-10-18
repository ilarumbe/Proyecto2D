using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedCrystalController : MonoBehaviour
{
    public int valor = 1;
    public GameManager gameManager;
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            gameManager.SumarCristales(valor);
            Destroy(gameObject);
        }
    }
}
