using UnityEngine;

public class ControlBrazosYoshi : MonoBehaviour
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
        // IZQUIERDA (Q)
        if (Input.GetKey(KeyCode.Q))
        {
            brazoIzquierdo.localRotation = Quaternion.Slerp(
                brazoIzquierdo.localRotation,
                rotacionInicialIzq * Quaternion.Euler(rotacionArriba, 0, 0),
                Time.deltaTime * velocidad
            );

            if (ShyGuyController.manoActual != true)
            {
                ShyGuyController.juegoActivo = false;
            }

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

        // DERECHA (E)
        if (Input.GetKey(KeyCode.E))
        {
            brazoDerecho.localRotation = Quaternion.Slerp(
                brazoDerecho.localRotation,
                rotacionInicialDer * Quaternion.Euler(rotacionArriba, 0, 0),
                Time.deltaTime * velocidad
            );

            if (ShyGuyController.manoActual != false)
            {
                ShyGuyController.juegoActivo = false;
            }

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
