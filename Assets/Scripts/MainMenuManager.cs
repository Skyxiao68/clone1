using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public GameObject mainPanel;
    public GameObject settingsPanel;

    public void PlayGame()
    {
        SceneManager.LoadScene("Map");
    }
    
public void OpenSettings()
{
    mainPanel.SetActive(false);
    settingsPanel.SetActive(true);

}

public void Back()
{
    settingsPanel.SetActive(false);
    mainPanel.SetActive(true);
}

public void QuitGame()
{
    Application.Quit();
}

  
}
