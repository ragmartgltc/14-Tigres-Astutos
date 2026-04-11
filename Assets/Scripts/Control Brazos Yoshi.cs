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
        // Brazo izquierdo (tecla Q)
        if (Input.GetKey(KeyCode.Q))
        {
            brazoIzquierdo.localRotation = Quaternion.Slerp(
                brazoIzquierdo.localRotation,
                rotacionInicialIzq * Quaternion.Euler(rotacionArriba, 0, 0),
                Time.deltaTime * velocidad
            );
        }
        else
        {
            brazoIzquierdo.localRotation = Quaternion.Slerp(
                brazoIzquierdo.localRotation,
                rotacionInicialIzq,
                Time.deltaTime * velocidad
            );
        }

        // Brazo derecho (tecla E)
        if (Input.GetKey(KeyCode.E))
        {
            brazoDerecho.localRotation = Quaternion.Slerp(
                brazoDerecho.localRotation,
                rotacionInicialDer * Quaternion.Euler(rotacionArriba, 0, 0),
                Time.deltaTime * velocidad
            );
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
