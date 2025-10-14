using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float velocity = 5f;
    public float jumpForce = 5f;
    public float rayLength = 0.5f;
    public Transform estaEnSuelo;
    public LayerMask groundLayer;
    Rigidbody2D rb;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        float inputMovimiento = Input.GetAxis("Horizontal");
        gestionarMovimiento(inputMovimiento);
    }

    void gestionarMovimiento(float inputMovimiento)
    {
        rb.velocity = new Vector2(inputMovimiento * velocity, rb.velocity.y);

        RaycastHit2D hit = Physics2D.Raycast(estaEnSuelo.position, Vector2.down, rayLength, groundLayer);
        if (hit.collider != null && Input.GetKeyDown(KeyCode.Space))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }

}
