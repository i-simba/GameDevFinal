using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuHandler : MonoBehaviour
{
    [SerializeField] GameObject optionsCanvas;

    public void PlayGame()
    {
        Click();
        SceneManager.LoadScene("Game");
    }

    public void QuitGame()
    {
        Click();
        Debug.Log("Quitting Game ...");
        Application.Quit();
    }

    public void MainMenu()
    {
        Click();
        SceneManager.LoadScene("MainMenu");
    }

    public void Options()
    {
        optionsCanvas.GetComponent<Canvas>().enabled = true;
    }

    private void Click()
    {
        MainMenuAudio.manager.PlaySFX( MainMenuAudio.manager.click ); // Erroring in death
    }
}
