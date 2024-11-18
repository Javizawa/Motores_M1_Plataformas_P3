using System.Collections;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [Header("Movement Settings")]
    public bool moveInX = true; // Activar movimiento en el eje X
    public bool moveInY = false; // Activar movimiento en el eje Y
    public float speed = 2f; // Velocidad de movimiento
    public float xLimit = 5f; // Distancia límite en X desde la posición inicial
    public float yLimit = 5f; // Distancia límite en Y desde la posición inicial
    public float pauseDuration = 1f; // Tiempo de espera en cada extremo

    private Vector3 _startPosition; // Posición inicial de la plataforma
    private Vector3 _targetPosition; // Posición objetivo
    private bool _movingToTarget = true; // Dirección del movimiento
    private float _pauseTimer = 0f;

    private void Start()
    {
        // Guardamos la posición inicial
        _startPosition = transform.position;

        // Calculamos la posición objetivo inicial
        _targetPosition = new Vector3(
            moveInX ? _startPosition.x + xLimit : _startPosition.x,
            moveInY ? _startPosition.y + yLimit : _startPosition.y,
            _startPosition.z
        );
    }

    private void Update()
    {
        if (_pauseTimer > 0f)
        {
            _pauseTimer -= Time.deltaTime;
            return;
        }

        // Movimiento de la plataforma
        MovePlatform();
    }

    private void MovePlatform()
    {
        // Calculamos la dirección del movimiento
        Vector3 target = _movingToTarget ? _targetPosition : _startPosition;
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);

        // Verificamos si ha llegado al destino
        if (Vector3.Distance(transform.position, target) < 0.01f)
        {
            // Cambiamos la dirección
            _movingToTarget = !_movingToTarget;
            _pauseTimer = pauseDuration;
        }
    }

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying)
        {
            // Dibujamos las posiciones límites en el editor
            Vector3 startPos = transform.position;
            Vector3 targetPos = new Vector3(
                moveInX ? startPos.x + xLimit : startPos.x,
                moveInY ? startPos.y + yLimit : startPos.y,
                startPos.z
            );

            Gizmos.color = Color.green;
            Gizmos.DrawLine(startPos, targetPos);
        }
    }
}