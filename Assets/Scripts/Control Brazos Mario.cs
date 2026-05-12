using UnityEngine;

public class ControlBrazosMario : MonoBehaviour
{
    public Transform brazoIzquierdo;
    public Transform brazoDerecho;

    public float rotacionArriba = -60f;
    public float velocidad = 5f;

    private Quaternion rotacionInicialIzq;
    private Quaternion rotacionInicialDer;

    void Start()
    {
        rotacionInicialIzq = brazoIzquierdo.localRotation;
        rotacionInicialDer = brazoDerecho.localRotation;
    }

    void Update()
    {
        // IZQUIERDA (I)
        if (Input.GetKey(KeyCode.I))
        {
            brazoIzquierdo.localRotation = Quaternion.Slerp(
                brazoIzquierdo.localRotation,
                rotacionInicialIzq * Quaternion.Euler(rotacionArriba, 0, 0),
                Time.deltaTime * velocidad
            );

            // ❌ FALLO si Shy Guy no estaba en izquierda
            if (ShyGuyController.manoActual != true)
            {
                ShyGuyController.juegoActivo = false;
            }

            // ✔️ responde ronda
            ShyGuyController.esperandoRespuesta = false;
        }
        else
        {
            brazoIzquierdo.localRotation = Quaternion.Slerp(
                brazoIzquierdo.localRotation,
                rotacionInicialIzq,
                Time.deltaTime * velocidad
            );
        }

        // DERECHA (P)
        if (Input.GetKey(KeyCode.P))
        {
            brazoDerecho.localRotation = Quaternion.Slerp(
                brazoDerecho.localRotation,
                rotacionInicialDer * Quaternion.Euler(rotacionArriba, 0, 0),
                Time.deltaTime * velocidad
            );

            // ❌ FALLO si Shy Guy no estaba en derecha
            if (ShyGuyController.manoActual != false)
            {
                ShyGuyController.juegoActivo = false;
            }

            // ✔️ responde ronda
            ShyGuyController.esperandoRespuesta = false;
        }
        else
        {
            brazoDerecho.localRotation = Quaternion.Slerp(
                brazoDerecho.localRotation,
                rotacionInicialDer,
                Time.deltaTime * velocidad
            );
        }
    }
}