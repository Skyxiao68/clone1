using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
   public GameObject pausePanel;
    public GameObject settingsPanel;
    public GameObject pauseButton;
    private bool isPaused = false;
    
   public void LoadTutorial()
    {
        SceneManager.LoadScene("TutorialScreen");
    }
    
    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadLevelCleared()
    {
        SceneManager.LoadScene("LevelCleared");
    }
    
    public void QuitGame()
    {
        Application.Quit();
    }
    
    

    public void LoadMainGameScreen()
    {
        SceneManager.LoadScene("MainGame");
    }

    

    public void TogglePause()
    {
        isPaused = !isPaused;
        pausePanel.SetActive(isPaused);
        Time.timeScale = isPaused ? 0f : 1f;
    }

    public void ResumeGame()
    {
        isPaused = false;
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
    }
    
   
    
    
    
    

   
}