using UnityEngine;
using UnityEngine.SceneManagement;

public class gameend: MonoBehaviour
{
    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void MainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Main Menu"); 
    }
    
    public void Quit()
    {
        Application.Quit(); 
    }
}