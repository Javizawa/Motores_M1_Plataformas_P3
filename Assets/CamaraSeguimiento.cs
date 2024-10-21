using UnityEngine;

public class CamaraSeguimiento : MonoBehaviour
{
    public Transform personaje; // Referencia al personaje a seguir
    public Vector2 margen;      // Margen horizontal y vertical que la cámara puede tener antes de moverse
    public Vector2 suavizado;   // Cuánto tarda la cámara en moverse al seguir al personaje

    private Vector3 velocidadActual; // Velocidad actual de la cámara
    private Camera camara;

    void Start()
    {
        camara = Camera.main;
    }


    void FixedUpdate()
    {
        // Obtenemos la posición actual de la cámara
        Vector3 posicionDeseada = transform.position;


        Vector2 posicionCamara = CalcularLimitesX();


        if (posicionDeseada.x >posicionCamara.x - 10) { 
            posicionDeseada.x = Mathf.SmoothDamp(transform.position.x, personaje.position.x, ref velocidadActual.x, suavizado.x);
        }
        else if(posicionDeseada.x < posicionCamara.x + 10)
        {
            posicionDeseada.x = Mathf.SmoothDamp(transform.position.x, personaje.position.x, ref velocidadActual.x, suavizado.x);
        }
        // Limitar la posición de la cámara a los valores mínimos y máximos
        //  posicionDeseada.x = Mathf.Clamp(posicionDeseada.x, minPosicion.x, maxPosicion.x);
        //  posicionDeseada.y = Mathf.Clamp(posicionDeseada.y, minPosicion.y, maxPosicion.y);

        // Actualizar la posición de la cámara
        transform.position = new Vector3(posicionDeseada.x, posicionDeseada.y, transform.position.z);
    }



    Vector2 CalcularLimitesX()
    {
        float camaraAltura = camara.orthographicSize;
        float camaraAnchura = camaraAltura * camara.aspect;

        float xMinima = camara.transform.position.x - camaraAnchura;
        float xMaxima = camara.transform.position.x + camaraAnchura;

        return new Vector2(xMinima, xMaxima);
    }

}