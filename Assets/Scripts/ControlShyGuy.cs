using UnityEngine;
using System.Collections;
using TMPro;

public class ShyGuyController : MonoBehaviour
{
    // =========================================
    // BRAZOS
    // =========================================
    [Header("Brazos")]
    public Transform brazoIzquierdo;
    public Transform brazoDerecho;

    // =========================================
    // CUERPO
    // =========================================
    [Header("Cuerpo")]
    public Transform cuerpoVisual;

    // =========================================
    // UI
    // =========================================
    [Header("UI")]
    public TextMeshPro countdownText;
    public TextMeshPro estadoText;
    public TextMeshPro puntajeText;

    // =========================================
    // AUDIO
    // =========================================
    [Header("Audio")]
    public AudioSource audioSource;

    public AudioClip sonidoCuenta;
    public AudioClip sonidoGo;
    public AudioClip sonidoLevantarBrazo;
    public AudioClip sonidoCorrecto;
    public AudioClip sonidoIncorrecto;
    public AudioClip sonidoGameOver;

    // =========================================
    // CONFIGURACIÓN
    // =========================================
    [Header("Configuración")]
    public float rotacionArriba = -60f;

    public float tiempoEntreRondas = 3f;
    public float tiempoMostrarMano = 0.8f;

    public float velocidadSuavizado = 8f;

    // =========================================
    // EFECTO NATURAL
    // =========================================
    [Header("Efecto Natural")]
    public float inclinacionCuerpo = 6f;
    public float velocidadCuerpo = 5f;

    // =========================================
    // CUENTA REGRESIVA
    // =========================================
    [Header("Cuenta Regresiva")]
    public float tiempoCuentaRegresiva = 1f;

    // =========================================
    // ROTACIONES
    // =========================================
    private Quaternion rotacionInicialIzq;
    private Quaternion rotacionInicialDer;
    private Quaternion rotacionInicialCuerpo;

    private Quaternion targetIzq;
    private Quaternion targetDer;
    private Quaternion targetCuerpo;

    // =========================================
    // ESTADO GLOBAL
    // =========================================
    public static bool manoActual; // true = izquierda
    public static bool esperandoRespuesta = false;

    public static bool juegoActivo = true;
    public static bool puedeJugar = false;

    // =========================================
    // PUNTAJE
    // =========================================
    public static int puntuacion = 0;

    // =========================================
    // START
    // =========================================
    void Start()
    {
        // Guardar rotaciones iniciales
        rotacionInicialIzq = brazoIzquierdo.localRotation;
        rotacionInicialDer = brazoDerecho.localRotation;

        rotacionInicialCuerpo = cuerpoVisual.localRotation;

        // Targets iniciales
        targetIzq = rotacionInicialIzq;
        targetDer = rotacionInicialDer;
        targetCuerpo = rotacionInicialCuerpo;

        // UI inicial
        countdownText.gameObject.SetActive(true);

        estadoText.text = "";
        puntajeText.text = "PUNTAJE: 0";

        // Iniciar juego
        StartCoroutine(IniciarJuego());
    }

    // =========================================
    // UPDATE
    // =========================================
    void Update()
    {
        // =========================================
        // BRAZO IZQUIERDO
        // =========================================
        brazoIzquierdo.localRotation = Quaternion.Slerp(
            brazoIzquierdo.localRotation,
            targetIzq,
            Time.deltaTime * velocidadSuavizado
        );

        // =========================================
        // BRAZO DERECHO
        // =========================================
        brazoDerecho.localRotation = Quaternion.Slerp(
            brazoDerecho.localRotation,
            targetDer,
            Time.deltaTime * velocidadSuavizado
        );

        // =========================================
        // CUERPO
        // =========================================
        cuerpoVisual.localRotation = Quaternion.Slerp(
            cuerpoVisual.localRotation,
            targetCuerpo,
            Time.deltaTime * velocidadCuerpo
        );
    }

    // =========================================
    // CUENTA REGRESIVA
    // =========================================
    IEnumerator IniciarJuego()
    {
        puedeJugar = false;

        // 3
        countdownText.text = "3";
        audioSource.PlayOneShot(sonidoCuenta);

        yield return new WaitForSeconds(tiempoCuentaRegresiva);

        // 2
        countdownText.text = "2";
        audioSource.PlayOneShot(sonidoCuenta);

        yield return new WaitForSeconds(tiempoCuentaRegresiva);

        // 1
        countdownText.text = "1";
        audioSource.PlayOneShot(sonidoCuenta);

        yield return new WaitForSeconds(tiempoCuentaRegresiva);

        // GO
        countdownText.text = "¡GO!";
        audioSource.PlayOneShot(sonidoGo);

        yield return new WaitForSeconds(1f);

        countdownText.gameObject.SetActive(false);

        puedeJugar = true;

        StartCoroutine(BucleJuego());
    }

    // =========================================
    // BUCLE PRINCIPAL
    // =========================================
    IEnumerator BucleJuego()
    {
        while (juegoActivo)
        {
            estadoText.text = "Observa atentamente...";

            yield return new WaitForSeconds(tiempoEntreRondas);

            // Mano aleatoria
            manoActual = Random.value < 0.5f;

            Debug.Log("Shy Guy: " + (manoActual ? "IZQUIERDA" : "DERECHA"));

            // Mostrar mano
            MostrarMano();

            audioSource.PlayOneShot(sonidoLevantarBrazo);

            yield return new WaitForSeconds(tiempoMostrarMano);

            // Reset
            ResetManos();

            // Esperar respuesta
            esperandoRespuesta = true;

            estadoText.text = "¡Responde!";

            yield return new WaitUntil(() => esperandoRespuesta == false);

            // Dificultad progresiva
            if (puntuacion >= 5)
            {
                tiempoEntreRondas = 2f;
            }

            if (puntuacion >= 10)
            {
                tiempoEntreRondas = 1.5f;
            }
        }

        GameOver();
    }

    // =========================================
    // MOSTRAR MANO
    // =========================================
    void MostrarMano()
    {
        if (manoActual)
        {
            // IZQUIERDA
            targetIzq = rotacionInicialIzq *
                        Quaternion.Euler(rotacionArriba, 0, 0);

            targetDer = rotacionInicialDer;
        }
        else
        {
            // DERECHA
            targetIzq = rotacionInicialIzq;

            targetDer = rotacionInicialDer *
                        Quaternion.Euler(-rotacionArriba, 0, 0);
        }

        // Movimiento natural del cuerpo
        float dir = manoActual ? -1f : 1f;

        targetCuerpo = rotacionInicialCuerpo *
                        Quaternion.Euler(0, 0, inclinacionCuerpo * dir);
    }

    // =========================================
    // RESET
    // =========================================
    void ResetManos()
    {
        targetIzq = rotacionInicialIzq;
        targetDer = rotacionInicialDer;

        targetCuerpo = rotacionInicialCuerpo;
    }

    // =========================================
    // RESPUESTA CORRECTA
    // =========================================
    public void RespuestaCorrecta()
    {
        puntuacion++;

        puntajeText.text = "PUNTAJE: " + puntuacion;

        estadoText.text = "¡Correcto!";

        audioSource.PlayOneShot(sonidoCorrecto);

        esperandoRespuesta = false;
    }

    // =========================================
    // RESPUESTA INCORRECTA
    // =========================================
    public void RespuestaIncorrecta()
    {
        juegoActivo = false;

        puedeJugar = false;

        estadoText.text = "¡Incorrecto!";

        audioSource.PlayOneShot(sonidoIncorrecto);

        esperandoRespuesta = false;
    }

    // =========================================
    // GAME OVER
    // =========================================
    void GameOver()
    {
        estadoText.text =
            "GAME OVER\n" +
            "PUNTAJE FINAL: " + puntuacion +
            "\nPresiona R para reiniciar";

        audioSource.PlayOneShot(sonidoGameOver);
    }
}