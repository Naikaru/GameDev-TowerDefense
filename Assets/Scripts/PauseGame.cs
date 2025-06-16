using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    private bool isPaused;

    void Awake()
    {
        // Start game outside of Pause menu
        isPaused = false;
        pauseMenu.gameObject.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            isPaused = !isPaused;
            pauseMenu.gameObject.SetActive(isPaused);
            Time.timeScale = isPaused ? 0f : 1f;
        }
    }

    public void GoBackToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
