using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void Play()
    {
        SceneManager.LoadScene("TowerDefenseGame");
    }

    public void Settings()
    {

    }

    public void Quit()
    {
        Application.Quit();
    }
}
