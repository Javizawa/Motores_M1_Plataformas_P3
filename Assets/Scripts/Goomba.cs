using System.Collections;
using UnityEngine;

public class Goomba : MonoBehaviour
{
    public bool isDetected;

    [SerializeField]
    private int velocidad = 10;
    private float positionY = 0;
    private bool delay;
    private float size;
    private GameObject personaje;
    private bool direccion = true;

    private void Start()
    {
        isDetected = false;
        delay = false;
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
        if (detectedBottom.collider != null)
        {
            positionY = 0;
        }
        else
        {
            if (!delay)
            {
                delay = true;
                StartCoroutine(ExecuteAfterTime(0.3f));
            }
        }
    }

    IEnumerator ExecuteAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        positionY = -10 * Time.deltaTime;
        delay = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        size = transform.position.y;

        if (collision.gameObject.tag == "Personaje")
        {
            if (size < collision.transform.position.y)
            {
                personaje.GetComponent<Rigidbody2D>().velocity = Vector2.up * 30;
                velocidad = 0;
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
            Debug.Log(detected.collider.tag);
        }

        if (detected)
        {
            Debug.Log("AQUI");
            direccion = !direccion;
            velocidad *= -1;
            Voltear(velocidad > 0 ? 1 : -1); // Ajustar dirección visual al chocar con plataformas
        }

    }

    private void MuereEnemigo()
    {
        Destroy(this.gameObject);
    }
}