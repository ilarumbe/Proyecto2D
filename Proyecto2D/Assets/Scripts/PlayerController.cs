using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float velocity = 10f;
    public float jumpForce = 5f;
    public float rayLength = 0.5f;
    public Transform estaEnSuelo;
    public LayerMask groundLayer;
    private Rigidbody2D rb;
    private Animator animator;
    private bool enSuelo;

    // DASH
    public float dashDistance = 5f;
    public float dashDuration = 0.15f;
    private bool estabaEnSuelo;
    private bool estaDasheando = false;
    private Vector2 dashDirection;
    private float dashTimer;
    private Vector3 dashStartPos;
    private Vector3 dashEndPos;
    private bool puedeDashear = true;
    private bool dashHaciaArriba = false;

    // PARTICULAS
    public ParticleSystem walkParticles;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        float inputMovimiento = Input.GetAxis("Horizontal");

        if (!estaDasheando)
        {
            GestionarMovimiento(inputMovimiento);
            GestionarGiro(inputMovimiento);
            RevisarSuelo();
            Saltar();
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && puedeDashear)
        {
            IniciarDash();
        }

        GestionarDash();
        GestionarParticulasAndar(inputMovimiento);

        animator.SetBool("enSuelo", enSuelo);
    }

    void GestionarMovimiento(float input)
    {
        rb.velocity = new Vector2(input * velocity, rb.velocity.y);
        animator.SetBool("enMovimiento", input != 0);
    }

    void GestionarGiro(float input)
    {
        if (input > 0)
            transform.localScale = new Vector3(1, 1, 1);
        else if (input < 0)
            transform.localScale = new Vector3(-1, 1, 1);
    }

    void RevisarSuelo()
    {
        RaycastHit2D hit = Physics2D.Raycast(estaEnSuelo.position, Vector2.down, rayLength, groundLayer);
        enSuelo = hit.collider != null;

        if (enSuelo)
        {
            puedeDashear = true;
            dashHaciaArriba = false;
        }

        estabaEnSuelo = enSuelo;
    }

    void Saltar()
    {
        if (enSuelo && Input.GetKeyDown(KeyCode.Space))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }

    void IniciarDash()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        if (x == 0 && y == 0)
            x = transform.localScale.x;

        dashDirection = new Vector2(x, y).normalized;

        dashStartPos = transform.position;
        dashEndPos = dashStartPos + (Vector3)(dashDirection * dashDistance);

        rb.velocity = Vector2.zero;
        rb.gravityScale = 0;

        estaDasheando = true;
        dashTimer = 0f;

        if (y > 0)
        {
            dashHaciaArriba = true;
            puedeDashear = false;
        }
        else if (!enSuelo)
        {
            puedeDashear = false;
        }

        animator.SetBool("dash", true);

    }

    void GestionarDash()
    {
        if (estaDasheando)
        {
            dashTimer += Time.deltaTime;
            float t = dashTimer / dashDuration;
            transform.position = Vector3.Lerp(dashStartPos, dashEndPos, t);

            if (t >= 1f)
            {
                TerminarDash();
            }
        }
    }

    void TerminarDash()
    {
        estaDasheando = false;
        rb.gravityScale = 1;
        animator.SetBool("dash", false);
    }

    void GestionarParticulasAndar(float input)
    {
        if (walkParticles == null) return;

        if (enSuelo && Mathf.Abs(input) > 0.1f && !estaDasheando)
        {
            if (!walkParticles.isPlaying) walkParticles.Play();
        }
        else
        {
            if (walkParticles.isPlaying) walkParticles.Stop();
        }
    }

}
