using UnityEngine;

public class ControlBrazosMario : MonoBehaviour
{
    [Header("Referencias")]
    public ShyGuyController shyGuy;

    [Header("Brazos")]
    public Transform brazoIzquierdo;
    public Transform brazoDerecho;

    [Header("Movimiento")]
    public float rotacionArriba = -60f;
    public float velocidad = 5f;

    [Header("Audio")]
    public AudioSource audioSource;

    public AudioClip sonidoLevantarBrazo;
    public AudioClip sonidoCorrecto;
    public AudioClip sonidoIncorrecto;

    private Quaternion rotacionInicialIzq;
    private Quaternion rotacionInicialDer;

    void Start()
    {
        if (shyGuy == null)
        {
            shyGuy = FindObjectOfType<ShyGuyController>();
        }

        rotacionInicialIzq = brazoIzquierdo.localRotation;
        rotacionInicialDer = brazoDerecho.localRotation;
    }

    void Update()
    {
        if (!ShyGuyController.puedeJugar)
            return;

        // ==========================
        // TECLA I (IZQUIERDA)
        // ==========================
        if (Input.GetKeyDown(KeyCode.I))
        {
            audioSource.PlayOneShot(sonidoLevantarBrazo);

            brazoIzquierdo.localRotation =
                rotacionInicialIzq *
                Quaternion.Euler(rotacionArriba, 0, 0);

            if (ShyGuyController.manoActual)
            {
                audioSource.PlayOneShot(sonidoCorrecto);

                if (shyGuy != null)
                    shyGuy.RespuestaCorrecta();
            }
            else
            {
                audioSource.PlayOneShot(sonidoIncorrecto);

                if (shyGuy != null)
                    shyGuy.RespuestaIncorrecta();
            }
        }

        // ==========================
        // TECLA P (DERECHA)
        // ==========================
        if (Input.GetKeyDown(KeyCode.P))
        {
            audioSource.PlayOneShot(sonidoLevantarBrazo);

            brazoDerecho.localRotation =
                rotacionInicialDer *
                Quaternion.Euler(rotacionArriba, 0, 0);

            if (!ShyGuyController.manoActual)
            {
                audioSource.PlayOneShot(sonidoCorrecto);

                if (shyGuy != null)
                    shyGuy.RespuestaCorrecta();
            }
            else
            {
                audioSource.PlayOneShot(sonidoIncorrecto);

                if (shyGuy != null)
                    shyGuy.RespuestaIncorrecta();
            }
        }

        // ==========================
        // VOLVER A POSICIÓN INICIAL
        // ==========================
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
}