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
    Animator animator;
    bool enSuelo;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        float inputMovimiento = Input.GetAxis("Horizontal");
        gestionarMovimiento(inputMovimiento);
        gestionarGiro(inputMovimiento);
        salto();
        animator.SetBool("enSuelo", enSuelo);
    }

    void gestionarMovimiento(float inputMovimiento)
    {
        rb.velocity = new Vector2(inputMovimiento * velocity, rb.velocity.y);

        if (inputMovimiento != 0)
        {
            animator.SetBool("enMovimiento", true);
        }
        else
        {
            animator.SetBool("enMovimiento", false);
        }
    }

    void gestionarGiro(float inputMovimiento)
    {
        if (inputMovimiento > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (inputMovimiento < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    void salto()
    {
        RaycastHit2D hit = Physics2D.Raycast(estaEnSuelo.position, Vector2.down, rayLength, groundLayer);
        enSuelo = hit.collider != null;
        if (enSuelo && Input.GetKeyDown(KeyCode.Space))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }

}
