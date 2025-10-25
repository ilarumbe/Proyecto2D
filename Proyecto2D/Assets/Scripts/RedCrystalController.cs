using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedCrystalController : MonoBehaviour
{
    private bool recogido = false;
    public int valor = 1;
    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        if (recogido) return;

        recogido = true;
        Destroy(gameObject);
    }
}
