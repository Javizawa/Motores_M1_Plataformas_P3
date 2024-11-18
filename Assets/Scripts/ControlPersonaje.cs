using UnityEngine;

public class ControlPersonaje : MonoBehaviour
{
    public float velocidadMovimiento = 5f;
    public float fuerzaSalto = 5f;
    public float tiempoMaximoSalto = 0.5f; // Duración máxima que se puede mantener el salto
    private float tiempoSaltoActual = 0f;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private bool estaSaltando = false;
    private bool botonSaltoPresionado = false;
    private Animator animator;
    private int direction = 1;
    private float originalXScale;
    private float xVelocity;

    private Transform plataformaActual = null; // Para manejar la plataforma actual

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        originalXScale = transform.localScale.x;
    }

    void Update()
    {
        MovimientoPersonaje();
        SaltoPersonaje();
    }

    private bool EstaEnSuelo()
    {
        // Verificar si el personaje está tocando el suelo
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1.1f, groundLayer);
        return hit.collider != null;
    }

    private void MovimientoPersonaje()
    {
        // Movimiento horizontal
        float movimientoHorizontal = Input.GetAxis("Horizontal");

        // Calcula la velocidad deseada
        xVelocity = velocidadMovimiento * movimientoHorizontal;

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

        rb.velocity = new Vector2(movimientoHorizontal * velocidadMovimiento, rb.velocity.y);
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
            rb.velocity = new Vector2(rb.velocity.x, fuerzaSalto);
        }

        // Continuar el salto mientras se mantenga presionado el botón
        if (Input.GetButton("Jump") && estaSaltando && botonSaltoPresionado)
        {
            if (tiempoSaltoActual < tiempoMaximoSalto)
            {
                tiempoSaltoActual += Time.deltaTime;
                rb.velocity = new Vector2(rb.velocity.x, fuerzaSalto);
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
}