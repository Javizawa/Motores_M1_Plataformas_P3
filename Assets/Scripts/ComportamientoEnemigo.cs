using System.Collections;
using UnityEngine;

public class ComportamientoEnemigo : MonoBehaviour
{
    public bool isDetected;

    [SerializeField]
    private int velocidad = 10;
    private bool canChangeDirection = true;
    private float size;
    private GameObject personaje;
    private bool direccion = true;

    private void Start()
    {
        isDetected = false;
        personaje = GameObject.FindGameObjectWithTag("Personaje");
    }

    void Update()
    {
        if (!isDetected)
        {
            // Detectar al jugador a la izquierda
            RaycastHit2D detectedLeft = Physics2D.Raycast(transform.position, Vector2.left, 30f, LayerMask.GetMask("Jugador"));
            if (detectedLeft.collider != null)
            {
                isDetected = true;
                direccion = true;
                velocidad = Mathf.Abs(velocidad) * -1; // Mover a la izquierda
                Voltear(-1); // Girar a la izquierda
            }

            // Detectar al jugador a la derecha
            RaycastHit2D detectedRight = Physics2D.Raycast(transform.position, Vector2.right, 30f, LayerMask.GetMask("Jugador"));
            if (detectedRight.collider != null)
            {
                isDetected = true;
                direccion = false;
                velocidad = Mathf.Abs(velocidad); // Mover a la derecha
                Voltear(1); // Girar a la derecha
            }
        }else
            {
                CheckChangeDirection();
                transform.Translate(Vector2.right * velocidad * Time.deltaTime);
            }

        // Verificar si está cayendo
        IsFalling();
    }

    void IsFalling()
    {
        RaycastHit2D detectedBottom = Physics2D.Raycast(transform.position, Vector2.down, 1.3f, LayerMask.GetMask("Ground"));
        if (detectedBottom.collider == null)
        {
            canChangeDirection = false;
        }
        else
        {
            canChangeDirection = true;
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        size = collision.contacts[0].normal.y;

        if (collision.gameObject.tag == "Personaje")
        {
            if (size < -0.5f)
            {
                personaje.GetComponent<Rigidbody2D>().velocity = Vector2.up * 10;
                velocidad = 0;
                this.GetComponent<Animator>().SetBool("isDead", true);
                Invoke("MuereEnemigo", 0.5f);
            }
            else
            {
                Debug.Log("Te mata");
            }
        }
    }

    private void Voltear(int direccion)
    {
        // Voltear visualmente al enemigo
        transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x) * direccion, transform.localScale.y, transform.localScale.z);
    }


    private void CheckChangeDirection()
    {
        if (canChangeDirection) { 
            RaycastHit2D detected;
            if (direccion)
            {
                detected = Physics2D.Raycast(transform.position, Vector2.left, 1f, LayerMask.GetMask("Ground"));
            }
            else
            {
                detected = Physics2D.Raycast(transform.position, Vector2.right, 1f, LayerMask.GetMask("Ground"));
            }

            if (detected)
            {
                direccion = !direccion;
                velocidad *= -1;
                Voltear(velocidad > 0 ? 1 : -1); // Ajustar dirección visual al chocar con plataformas
            }
        }
    }

    private void MuereEnemigo()
    {
        Destroy(this.gameObject);
    }
}