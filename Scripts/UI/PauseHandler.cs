using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseHandler : MonoBehaviour
{
    [SerializeField] GameObject pauseObj;
    [SerializeField] GameObject optionsCanvas;
    [SerializeField] Image      pauseContainer;

    void Awake()
    {
        pauseContainer.transform.localPosition = Vector3.zero;
        optionsCanvas.GetComponent<Canvas>().enabled = false;
    }

    public void Resume()
    {
        pauseObj.GetComponent<Canvas>().enabled = false;
        EndOfRound.manager.isPaused = false;
        EndOfRound.canva.enabled = false;
        Time.timeScale = 1;
    }

    public void Options()
    {
        optionsCanvas.GetComponent<Canvas>().enabled = true;
    }

    public void MainMenu() {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }
}
