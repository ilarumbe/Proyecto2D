using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float velocity = 7f;
    public float jumpForce = 7f;
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
    public bool puedeDashear = true;
    private bool dashHaciaArriba = false;

    // PARTICULAS
    public ParticleSystem walkParticles;

    // ESTELA
    public GameObject ghostPrefab;
    public float ghostInterval = 0.05f;
    private float ghostTimer = 0f;
    public Color ghostColor = new Color(1f, 1f, 1f, 0.5f);
    public float ghostLifetime = 0.3f;
    private List<GameObject> activeGhosts = new List<GameObject>();

    //MUERTE
    private Vector3 posicionInicial;
    private bool estaMuriendo = false;
    private List<GameObject> blueBalls = new List<GameObject>();

    //HIELO
    private bool enHielo = false;
    public float friccionNormal = 20f;
    public float friccionHielo = 2f;
    private float velocidadSuavizada = 0f;

    //SONIDO
    public AudioSource audioPasos;
    public float velocidadMinimaPasos = 0.1f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        posicionInicial = transform.position;

        Collider2D collider = GetComponent<Collider2D>();
        if (collider != null)
        {
            PhysicsMaterial2D playerMaterial = new PhysicsMaterial2D();
            playerMaterial.friction = 0f;
            playerMaterial.bounciness = 0f;
            collider.sharedMaterial = playerMaterial;
        }

        if (ghostPrefab == null)
        {
            CreateDefaultGhostPrefab();
        }

        blueBalls.AddRange(GameObject.FindGameObjectsWithTag("BlueBall"));
    }

    void Update()
    {
        float inputMovimiento = Input.GetAxis("Horizontal");

        if (estaMuriendo)
        return;

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
        GestionarEstelaFantasma();

        animator.SetBool("enSuelo", enSuelo);
    }

    void GestionarMovimiento(float input)
    {
        float friccionActual = enHielo ? friccionHielo : friccionNormal;

        if (input != 0)
            velocidadSuavizada = Mathf.MoveTowards(velocidadSuavizada, input * velocity, friccionActual * Time.deltaTime);
        else
            velocidadSuavizada = Mathf.MoveTowards(velocidadSuavizada, 0, friccionActual * Time.deltaTime);

        rb.velocity = new Vector2(velocidadSuavizada, rb.velocity.y);

        bool enMovimiento = Mathf.Abs(velocidadSuavizada) > velocidadMinimaPasos && enSuelo && !estaDasheando;
        animator.SetBool("enMovimiento", enMovimiento);

        if (enMovimiento)
        {
            if (!audioPasos.isPlaying)
                audioPasos.Play();
        }
        else
        {
            if (audioPasos.isPlaying)
                audioPasos.Stop();
        }
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
        ghostTimer = 0f;

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

            Vector2 dir = dashDirection.normalized;
            float dashSpeed = dashDistance / dashDuration;

            rb.velocity = dir * dashSpeed;

            if (dashTimer >= dashDuration)
            {
                TerminarDash();
            }
        }
    }

    void TerminarDash()
    {
        estaDasheando = false;
        rb.velocity = Vector2.zero;
        rb.gravityScale = 1;
        animator.SetBool("dash", false);
        LimpiarFantasmas();
    }

    void GestionarParticulasAndar(float input)
    {
        if (walkParticles == null) return;

        if (enSuelo && Mathf.Abs(input) > 0.1f && !estaDasheando)
        {
            if (!walkParticles.isPlaying)
            {
                walkParticles.Play();
                walkParticles.gameObject.SetActive(true);
            }
            
        }
        else
        {
            if (walkParticles.isPlaying)
            {
                walkParticles.Stop();
                walkParticles.gameObject.SetActive(false);
            }
        }
    }

    void GestionarEstelaFantasma()
    {
        if (estaDasheando)
        {
            ghostTimer += Time.deltaTime;

            if (ghostTimer >= ghostInterval)
            {
                CrearFantasma();
                ghostTimer = 0f;
            }
        }
    }

    void CrearFantasma()
    {
        if (ghostPrefab == null) return;

        GameObject ghost = Instantiate(ghostPrefab, transform.position, transform.rotation);

        ghost.transform.localScale = transform.localScale;

        SpriteRenderer ghostSprite = ghost.GetComponent<SpriteRenderer>();
        SpriteRenderer playerSprite = GetComponent<SpriteRenderer>();

        if (ghostSprite != null && playerSprite != null)
        {
            ghostSprite.sprite = playerSprite.sprite;

            Color azulClaro = new Color(0.4f, 0.9f, 1f, 0.5f);
            ghostSprite.color = azulClaro;

            ghostSprite.sortingOrder = playerSprite.sortingOrder - 1;
        }
        
        activeGhosts.Add(ghost);
        
        StartCoroutine(FadeAndDestroyGhost(ghost, ghostLifetime));
    }
    
    void LimpiarFantasmas()
    {
        foreach (GameObject ghost in activeGhosts)
        {
            if (ghost != null)
            {
                Destroy(ghost);
            }
        }
        activeGhosts.Clear();
    }

    private System.Collections.IEnumerator FadeAndDestroyGhost(GameObject ghost, float lifetime)
    {
        SpriteRenderer ghostSprite = ghost.GetComponent<SpriteRenderer>();
        float timer = lifetime;
        Color originalColor = ghostSprite.color;

        while (timer > 0f)
        {
            timer -= Time.deltaTime;
            
            if (ghostSprite != null)
            {
                Color newColor = originalColor;
                newColor.a = originalColor.a * (timer / lifetime);
                ghostSprite.color = newColor;
            }
            
            yield return null;
        }

        if (activeGhosts.Contains(ghost))
        {
            activeGhosts.Remove(ghost);
        }
        
        Destroy(ghost);
    }

    void CreateDefaultGhostPrefab()
    {
        ghostPrefab = new GameObject("DefaultGhost");
        SpriteRenderer sr = ghostPrefab.AddComponent<SpriteRenderer>();

        ghostPrefab.hideFlags = HideFlags.HideInHierarchy;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("BlueBall"))
        {
            puedeDashear = true;
            other.gameObject.SetActive(false);
        }
        if (other.CompareTag("BloqueMuerte") && !estaMuriendo)
        {
            StartCoroutine(MorirYReaparecer());
        }
        if (other.CompareTag("Cristal"))
        {
            FindObjectOfType<GameManager>().SumarCristales(1);
            Destroy(other.gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (estaDasheando)
        {
            TerminarDash();
        }

        foreach (ContactPoint2D contact in collision.contacts)
        {
            if (Mathf.Abs(contact.normal.x) > 0.7f)
            {
                rb.position += contact.normal * 0.02f;
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
{
    if (collision.collider.CompareTag("Hielo"))
        enHielo = true;
}

private void OnCollisionExit2D(Collision2D collision)
{
    if (collision.collider.CompareTag("Hielo"))
        enHielo = false;
}
    
    private IEnumerator MorirYReaparecer()
    {
        estaMuriendo = true;

        animator.SetTrigger("muerte");

        rb.velocity = Vector2.zero;
        rb.gravityScale = 0;
        GetComponent<Collider2D>().enabled = false;

        yield return new WaitForSeconds(0.5f);

        FindObjectOfType<GameManager>().SumarMuerte();

        transform.position = posicionInicial;

        foreach (GameObject b in blueBalls)
        {
            if (b != null)
            {
                b.SetActive(true);
            }
        }

        GetComponent<Collider2D>().enabled = true;
        rb.gravityScale = 1;
        rb.velocity = Vector2.zero;
        estaMuriendo = false;

        animator.Play("Idle");
    }


}
