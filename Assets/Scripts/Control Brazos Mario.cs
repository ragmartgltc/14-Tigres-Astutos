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
        // Brazo izquierdo (tecla I)
        if (Input.GetKey(KeyCode.I))
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

        // Brazo derecho (tecla P)
        if (Input.GetKey(KeyCode.P))
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