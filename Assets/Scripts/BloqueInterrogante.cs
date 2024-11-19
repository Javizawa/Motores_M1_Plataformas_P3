using UnityEngine;

public class BloqueInterrogante : MonoBehaviour
{
    [Header("Configuraciones")]
    public GameObject objetoASubir; // Objeto que subirá
    public float alturaSubida = 2f; // Altura en el eje Y
    public float velocidadSubida = 5f; // Velocidad de subida

    private bool estaSubiendo = false; // Para controlar la animación de subida
    private Vector3 posicionInicial; // La posición inicial del objeto
    private Vector3 posicionObjetivo; // La posición a la que subirá

    void Start()
    {
        if (objetoASubir != null)
        {
            posicionInicial = objetoASubir.transform.position;
            posicionObjetivo = posicionInicial;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Detectar si el objeto fue golpeado desde abajo
        if (collision.relativeVelocity.y > 0 && !estaSubiendo)
        {
            if (collision.collider.CompareTag("Personaje")) // Opcional: verifica si el golpe proviene de un objeto con tag específico
            {
                estaSubiendo = true; // Activar la animación de subida
                this.GetComponent<Animator>().SetTrigger("isTouched");
                posicionObjetivo = posicionInicial + Vector3.up * alturaSubida; // Definir la nueva posición objetivo
            }
        }
    }

    void Update()
    {
        if (estaSubiendo)
        {
            // Mover el objeto hacia la posición objetivo
            objetoASubir.transform.position = Vector3.MoveTowards(objetoASubir.transform.position, posicionObjetivo, velocidadSubida * Time.deltaTime);

            // Detener la animación cuando llegue a la posición objetivo
            if (Vector3.Distance(objetoASubir.transform.position, posicionObjetivo) < 0.01f)
            {
                estaSubiendo = false;
                posicionInicial = posicionObjetivo; // Actualizar la posición inicial
                this.gameObject.SetActive(false);
            }
        }
    }
}