using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Transform transform;
    public float horizontalVelocity = 3f;
    public float verticalVelocity = 3f;
    public float velocity = 5f;
    Rigidbody2D rb;
    bool enElSuelo = false;
    public float jumpForce = 0.1f;
    public float jumpVelocity = 0.1f;
    public Collider2D feetCollider;
    public LayerMask groundLayer;
    Animator animator;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        feetCollider = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        float inputMovimiento = Input.GetAxis("Horizontal");
        gestionarMovimiento(inputMovimiento);
    }

    void gestionarMovimiento(float inputMovimiento)
    {
        rb.velocity = new Vector2(inputMovimiento * velocity, rb.velocity.y);

        enElSuelo = feetCollider.IsTouchingLayers(groundLayer);
        if (enElSuelo && Input.GetKey(KeyCode.Space))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            enElSuelo = false;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.CompareTag("Suelo"))
        {
            enElSuelo = true;
        }
    }
}
