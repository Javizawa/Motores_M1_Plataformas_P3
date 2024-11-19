using TMPro;
using UnityEngine;

public class ControlPersonaje : MonoBehaviour
{
    public float velocidadMovimiento = 5f;
    public float fuerzaSalto = 5f;
    public float tiempoMaximoSalto = 0.5f; // Duración máxima que se puede mantener el salto
    private float tiempoSaltoActual = 0f;
    public LayerMask groundLayer;
    public TextMeshProUGUI livesNumber;
    public float radio = 0.4f;

    private Rigidbody2D rb;
    private bool estaSaltando = false;
    private bool botonSaltoPresionado = false;
    private Animator animator;
    private int direction = 1;
    private float originalXScale;
    private float xVelocity;
    private int lives = 3;
    private bool isVulnerable = true;
    private float vulnerabilityTime = 0f;

    private Transform plataformaActual = null; // Para manejar la plataforma actual

    public string boostButton = "Boost"; // Nombre del botón configurado en Unity
    public float boostMultiplier = 2f;  // Factor de aumento en velocidad y salto

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        originalXScale = transform.localScale.x;
        livesNumber.text = lives.ToString();
    }

    void Update()
    {
        MovimientoPersonaje();
        SaltoPersonaje();
        CheckVulnerability();
    }

    public bool GetVulneravility()
    {
        return isVulnerable;
    }

    public void SetVulnerability()
    {
        isVulnerable = false;
        vulnerabilityTime = Time.time;
    }

    private void CheckVulnerability()
    {
        if (!isVulnerable && (Time.time - vulnerabilityTime) > 3f)
        {
            isVulnerable = true;
        }
    }

    private bool EstaEnSuelo()
    {
        float anchoPersonaje = GetComponent<Collider2D>().bounds.extents.x;
        Vector2 origenCentro = new Vector2(transform.position.x, transform.position.y);

        RaycastHit2D hitCentro = Physics2D.Raycast(origenCentro, Vector2.down, 1.1f, groundLayer);
        Collider2D suelo = Physics2D.OverlapCircle(origenCentro, radio, groundLayer);

        Debug.DrawRay(origenCentro, Vector2.down * 1.1f, Color.green);

        return hitCentro.collider != null || suelo != null;
    }

    private void MovimientoPersonaje()
    {
        // Movimiento horizontal
        float movimientoHorizontal = Input.GetAxis("Horizontal");

        // Aumentar la velocidad si se presiona el botón de boost
        float velocidadActual = velocidadMovimiento;
        if (Input.GetButton(boostButton))
        {
            velocidadActual *= boostMultiplier;
        }

        // Calcula la velocidad deseada
        xVelocity = velocidadActual * movimientoHorizontal;

        // Comprueba si está moviéndose para activar la animación de correr
        animator.SetBool("Run", xVelocity != 0);

        // Si la dirección y el signo de la velocidad no coinciden, gira al personaje
        if (xVelocity * direction < 0f)
        {
            direction *= -1;
            Vector3 scale = transform.localScale;
            scale.x = originalXScale * direction;
            transform.localScale = scale;
        }

        rb.velocity = new Vector2(movimientoHorizontal * velocidadActual, rb.velocity.y);
    }

    private void SaltoPersonaje()
    {
        // Iniciar salto al presionar el botón
        if (Input.GetButtonDown("Jump") && EstaEnSuelo())
        {
            animator.SetBool("Jump", true);
            estaSaltando = true;
            botonSaltoPresionado = true;
            tiempoSaltoActual = 0f;

            // Aumentar la fuerza de salto si se presiona el botón de boost
            float fuerzaSaltoActual = fuerzaSalto;
            if (Input.GetButton(boostButton))
            {
                fuerzaSaltoActual *= boostMultiplier;
            }

            rb.velocity = new Vector2(rb.velocity.x, fuerzaSaltoActual);
        }

        // Continuar el salto mientras se mantenga presionado el botón
        if (Input.GetButton("Jump") && estaSaltando && botonSaltoPresionado)
        {
            if (tiempoSaltoActual < tiempoMaximoSalto)
            {
                tiempoSaltoActual += Time.deltaTime;

                // Aumentar la fuerza de salto continuo si se presiona el botón de boost
                float fuerzaSaltoActual = fuerzaSalto;
                if (Input.GetButton(boostButton))
                {
                    fuerzaSaltoActual *= boostMultiplier;
                }

                rb.velocity = new Vector2(rb.velocity.x, fuerzaSaltoActual);
            }
            else
            {
                botonSaltoPresionado = false; // El personaje ya no seguirá saltando aunque se mantenga el botón
            }
        }

        // Finalizar el salto cuando se suelta el botón
        if (Input.GetButtonUp("Jump"))
        {
            animator.SetBool("Jump", false);
            botonSaltoPresionado = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Detectar si estamos sobre una plataforma móvil
        if (collision.gameObject.CompareTag("PlataformaMovil"))
        {
            plataformaActual = collision.transform;
            transform.parent = plataformaActual; // Hacemos al personaje hijo de la plataforma
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // Detectar si dejamos de estar sobre la plataforma móvil
        if (collision.gameObject.CompareTag("PlataformaMovil"))
        {
            transform.parent = null; // Quitamos la relación con la plataforma
            plataformaActual = null;
        }
    }

    public void AddLives()
    {
        lives++;
        livesNumber.text = lives.ToString();
    }

    public int GetLives()
    {
        return lives;
    }

    public void SubtractLives()
    {
        lives--;
        livesNumber.text = lives.ToString();
    }
}