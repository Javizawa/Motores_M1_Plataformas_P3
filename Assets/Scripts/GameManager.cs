using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    float tiempoRestante = 10f;
    public TextMeshProUGUI time;

    void Start()
    {
        // Mostrar los segundos sin decimales al iniciar
        time.text = Mathf.FloorToInt(tiempoRestante).ToString();
    }

    void Update()
    {
        CheckTime();
    }

    void CheckTime()
    {
        if (Mathf.FloorToInt(tiempoRestante) > 0f)
        {
            tiempoRestante -= Time.deltaTime; // Reducir el tiempo restante
            // Mostrar solo los segundos sin decimales
            time.text = Mathf.FloorToInt(tiempoRestante).ToString();
        }
        else
        {
            Debug.Log("¡Se acabó el tiempo!");
        }
    }
}