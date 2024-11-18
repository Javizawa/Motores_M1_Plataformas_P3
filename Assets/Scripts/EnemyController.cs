using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float velocidadMovimiento = 2f; // Velocidad de movimiento del enemigo
    public float fuerzaSaltoPersonaje = 5f; // Fuerza del salto del personaje al matar al enemigo
    public LayerMask plataformasLayer; // Capa para detectar el suelo
    public float rangoDeteccionSuelo = 0.1f; // Distancia para detectar suelo debajo

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        // Verificar si hay suelo delante
        if (HaySueloDelante())
        {
            // Movimiento horizontal en una sola dirección
            rb.velocity = new Vector2(velocidadMovimiento, rb.velocity.y);
        }
        else
        {
            // Si no hay suelo, dejar que caiga
            rb.velocity = new Vector2(velocidadMovimiento, rb.velocity.y);
        }
    }

    private bool HaySueloDelante()
    {
        // Crear un Raycast justo delante del enemigo, en el borde inferior
        Vector2 posicionDelante = new Vector2(transform.position.x + 0.5f, transform.position.y);

        // Dibujar el Raycast para depuración
        Debug.DrawRay(posicionDelante, Vector2.down * rangoDeteccionSuelo, Color.red);

        // Detectar si hay suelo debajo
        return Physics2D.Raycast(posicionDelante, Vector2.down, rangoDeteccionSuelo, plataformasLayer);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Personaje"))
        {
            // Detectar la dirección del contacto
            Vector2 normal = collision.contacts[0].normal;

            if (normal.y < -0.5f) // Si el personaje salta encima
            {
                MatarEnemigo(collision.gameObject);
            }
            else
            {
                MatarPersonaje(collision.gameObject); // Colisión lateral o desde abajo
            }
        }
    }

    private void MatarEnemigo(GameObject personaje)
    {
        // Añadir un salto al personaje
        Rigidbody2D rbPersonaje = personaje.GetComponent<Rigidbody2D>();
        if (rbPersonaje != null)
        {
            rbPersonaje.velocity = new Vector2(rbPersonaje.velocity.x, fuerzaSaltoPersonaje);
        }

        // Destruir al enemigo
        Destroy(gameObject);
    }

    private void MatarPersonaje(GameObject personaje)
    {
        // Aquí puedes manejar la lógica de muerte del personaje
        Debug.Log("El personaje ha muerto.");
        Destroy(personaje);
    }
}