using UnityEngine;
using System.Collections;

public class YoshiAI : MonoBehaviour
{
    [Header("Brazos")]
    public Transform brazoIzquierdo;
    public Transform brazoDerecho;

    [Header("Movimiento")]
    public float rotacionArriba = -60f;
    public float velocidad = 5f;

    [Header("IA")]
    [Range(0f, 1f)]
    public float precision = 0.8f; // 80% de aciertos

    public float tiempoReaccionMin = 0.3f;
    public float tiempoReaccionMax = 1.2f;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip sonidoLevantarBrazo;
    public AudioClip sonidoCorrecto;
    public AudioClip sonidoIncorrecto;

    private Quaternion rotacionInicialIzq;
    private Quaternion rotacionInicialDer;

    private bool rondaDetectada = false;

    public int puntajeYoshi = 0;

    void Start()
    {
        rotacionInicialIzq = brazoIzquierdo.localRotation;
        rotacionInicialDer = brazoDerecho.localRotation;
    }

    void Update()
    {
        if (!ShyGuyController.puedeJugar)
            return;

        // Detectar UNA sola vez cada ronda
        if (ShyGuyController.esperandoRespuesta && !rondaDetectada)
        {
            rondaDetectada = true;
            StartCoroutine(Responder());
        }

        // Prepararse para la siguiente ronda
        if (!ShyGuyController.esperandoRespuesta)
        {
            rondaDetectada = false;
        }

        // Volver lentamente a posición inicial
        brazoIzquierdo.localRotation = Quaternion.Slerp(
            brazoIzquierdo.localRotation,
            rotacionInicialIzq,
            Time.deltaTime * velocidad
        );

        brazoDerecho.localRotation = Quaternion.Slerp(
            brazoDerecho.localRotation,
            rotacionInicialDer,
            Time.deltaTime * velocidad
        );
    }

    IEnumerator Responder()
    {
        float tiempoReaccion =
            Random.Range(tiempoReaccionMin, tiempoReaccionMax);

        yield return new WaitForSeconds(tiempoReaccion);

        // ¿Va a acertar?
        bool manoElegida;

        if (Random.value < precision)
        {
            // Copia correctamente a Shy Guy
            manoElegida = ShyGuyController.manoActual;
        }
        else
        {
            // Se equivoca
            manoElegida = !ShyGuyController.manoActual;
        }

        // Levantar brazo elegido
        if (manoElegida)
        {
            LevantarIzquierda();
        }
        else
        {
            LevantarDerecha();
        }

        // Comprobar resultado
        bool acerto =
            manoElegida == ShyGuyController.manoActual;

        if (acerto)
        {
            puntajeYoshi++;

            if (audioSource != null &&
                sonidoCorrecto != null)
            {
                audioSource.PlayOneShot(sonidoCorrecto);
            }

            Debug.Log("Yoshi acertó. Puntaje: " + puntajeYoshi);
        }
        else
        {
            if (audioSource != null &&
                sonidoIncorrecto != null)
            {
                audioSource.PlayOneShot(sonidoIncorrecto);
            }

            Debug.Log("Yoshi falló.");
        }
    }

    void LevantarIzquierda()
    {
        if (audioSource != null &&
            sonidoLevantarBrazo != null)
        {
            audioSource.PlayOneShot(sonidoLevantarBrazo);
        }

        brazoIzquierdo.localRotation =
            rotacionInicialIzq *
            Quaternion.Euler(rotacionArriba, 0, 0);
    }

    void LevantarDerecha()
    {
        if (audioSource != null &&
            sonidoLevantarBrazo != null)
        {
            audioSource.PlayOneShot(sonidoLevantarBrazo);
        }

        brazoDerecho.localRotation =
            rotacionInicialDer *
            Quaternion.Euler(rotacionArriba, 0, 0);
    }
}