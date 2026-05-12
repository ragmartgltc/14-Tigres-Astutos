using UnityEngine;
using System.Collections;

public class ShyGuyController : MonoBehaviour
{
    [Header("Brazos")]
    public Transform brazoIzquierdo;
    public Transform brazoDerecho;

    [Header("Cuerpo (objeto raíz del personaje)")]
    public Transform cuerpoVisual;

    [Header("Tiempos")]
    public float rotacionArriba = -60f;
    public float tiempoEntreRondas = 3f;
    public float tiempoMostrarMano = 0.8f;
    public float velocidadSuavizado = 8f;

    [Header("Efecto natural")]
    public float inclinacionCuerpo = 6f;
    public float velocidadCuerpo = 5f;

    private Quaternion rotacionInicialIzq;
    private Quaternion rotacionInicialDer;
    private Quaternion rotacionInicialCuerpo;

    private Quaternion targetIzq;
    private Quaternion targetDer;
    private Quaternion targetCuerpo;

    // Estado global (compatibilidad con otros scripts)
    public static bool manoActual; // true = izquierda, false = derecha
    public static bool esperandoRespuesta = false;
    public static bool juegoActivo = true;

    void Start()
    {
        rotacionInicialIzq = brazoIzquierdo.localRotation;
        rotacionInicialDer = brazoDerecho.localRotation;

        rotacionInicialCuerpo = cuerpoVisual.localRotation;

        targetIzq = rotacionInicialIzq;
        targetDer = rotacionInicialDer;
        targetCuerpo = rotacionInicialCuerpo;

        StartCoroutine(Bucle());
    }

    void Update()
    {
        // =========================
        // BRAZOS
        // =========================
        brazoIzquierdo.localRotation = Quaternion.Slerp(
            brazoIzquierdo.localRotation,
            targetIzq,
            Time.deltaTime * velocidadSuavizado
        );

        brazoDerecho.localRotation = Quaternion.Slerp(
            brazoDerecho.localRotation,
            targetDer,
            Time.deltaTime * velocidadSuavizado
        );

        // =========================
        // CUERPO (inclinación natural)
        // =========================
        cuerpoVisual.localRotation = Quaternion.Slerp(
            cuerpoVisual.localRotation,
            targetCuerpo,
            Time.deltaTime * velocidadCuerpo
        );
    }

    IEnumerator Bucle()
    {
        while (juegoActivo)
        {
            yield return new WaitForSeconds(tiempoEntreRondas);

            manoActual = Random.value < 0.5f;

            Debug.Log("Shy Guy: " + (manoActual ? "IZQUIERDA" : "DERECHA"));

            MostrarMano();

            yield return new WaitForSeconds(tiempoMostrarMano);

            ResetManos();

            esperandoRespuesta = true;
            yield return new WaitUntil(() => esperandoRespuesta == false);
        }
    }

    void MostrarMano()
    {
        if (manoActual)
        {
            // brazo izquierdo arriba
            targetIzq = rotacionInicialIzq *
                        Quaternion.Euler(rotacionArriba, 0, 0);

            targetDer = rotacionInicialDer;
        }
        else
        {
            // brazo derecho arriba
            targetIzq = rotacionInicialIzq;

            targetDer = rotacionInicialDer *
                        Quaternion.Euler(-rotacionArriba, 0, 0);
        }

        // =========================
        // CUERPO (CORREGIDO: contrapeso natural)
        // =========================
        float dir = manoActual ? -1f : 1f;

        targetCuerpo = rotacionInicialCuerpo *
                        Quaternion.Euler(0, 0, inclinacionCuerpo * dir);
    }

    void ResetManos()
    {
        targetIzq = rotacionInicialIzq;
        targetDer = rotacionInicialDer;
        targetCuerpo = rotacionInicialCuerpo;
    }
}