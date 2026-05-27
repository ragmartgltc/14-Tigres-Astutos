using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartGame : MonoBehaviour
{
    // Panel de Game Over (opcional)
    public GameObject gameOverPanel;

    // Variable de derrota
    public static bool gameOver = false;

    void Start()
    {
        // Reiniciar estado del juego
        gameOver = false;

        // Reactivar tiempo normal
        Time.timeScale = 1f;

        // Ocultar panel de derrota si existe
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }
    }

    void Update()
    {
        // Reiniciar con tecla R
        if (Input.GetKeyDown(KeyCode.R))
        {
            Restart();
        }
    }

    public void Restart()
    {
        // Restaurar tiempo
        Time.timeScale = 1f;

        // Reiniciar variable de derrota
        gameOver = false;

        // Recargar escena actual
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}