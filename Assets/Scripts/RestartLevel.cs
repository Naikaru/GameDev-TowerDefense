using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartLevel : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Restart level
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }        
    }
}
