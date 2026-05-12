using UnityEngine;

public class Animacionlevantarbrazomario : MonoBehaviour
{
    Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W)) // Cambia la tecla si lo deseas
        {
            animator.SetTrigger("LevantarBrazo");
        }
    }
}
