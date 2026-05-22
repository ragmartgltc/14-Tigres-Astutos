using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("Paneles")]
    public GameObject mainMenu;
    public GameObject optionsMenu;

    [Header("Audio")]
    public AudioSource audioSource;

    public AudioClip clickSound;
    public AudioClip hoverSound;

    [Header("Escena")]
    public string gameScene = "OutdoorsScene";

    private bool hoverPlaying = false;

    // ==========================
    // BOTONES
    // ==========================

    public void PlayGame()
    {
        PlayClickSound();

        Invoke(nameof(CargarJuego), 0.2f);
    }

    void CargarJuego()
    {
        SceneManager.LoadScene(gameScene);
    }

    public void OpenOptionsPanel()
    {
        PlayClickSound();

        mainMenu.SetActive(false);
        optionsMenu.SetActive(true);
    }

    public void OpenMainMenuPanel()
    {
        PlayClickSound();

        mainMenu.SetActive(true);
        optionsMenu.SetActive(false);
    }

    public void QuitGame()
    {
        PlayClickSound();

        Debug.Log("Saliendo del juego...");

        Application.Quit();
    }

    // ==========================
    // SONIDOS
    // ==========================

    public void PlayClickSound()
    {
        if (audioSource != null && clickSound != null)
        {
            audioSource.PlayOneShot(clickSound);
        }
    }

    public void PlayHoverSound()
    {
        if (audioSource != null &&
            hoverSound != null &&
            !hoverPlaying)
        {
            hoverPlaying = true;

            audioSource.PlayOneShot(hoverSound);

            Invoke(nameof(ResetHover), 0.1f);
        }
    }

    void ResetHover()
    {
        hoverPlaying = false;
    }
}