using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuBehaviour : MonoBehaviour
{
    // Start is called before the first frame update

    private GameObject PauseUi;
    private GameObject PauseUiCanvas;

    private GameObject MainUi;
    public bool IsGamePaused;
    void Awake()
    {
        IsGamePaused = false;
        
        PauseUi = gameObject;
        PauseUiCanvas = PauseUi.transform.GetChild(0).gameObject;
        PauseUiCanvas.SetActive(false);
        
        MainUi = GameObject.Find("Main User Interface");
    }

    void Update()
    {
        if ( IsGamePaused && (Input.GetKeyDown(InputManager.PauseMenuKey) || Input.GetKeyDown(KeyCode.Escape)) )
        {
            IsGamePaused = false;
            buttonResume();
        }
        else if (Input.GetKeyDown(InputManager.PauseMenuKey) || Input.GetKeyDown(KeyCode.Escape))
        {
            IsGamePaused = true;
            Pause();
        }
    }

    private void Pause()
    {
        MainUi.SetActive(false);
        PauseUiCanvas.SetActive(true);
        Time.timeScale = 0;
    }

    public void buttonResume()
    {
        PauseUiCanvas.SetActive(false);
        MainUi.SetActive(true);
        Time.timeScale = 1;
    }

    public void buttonOptions()
    {
        
    }

    public void buttonQuitToMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }

    public void buttonQuitGame()
    {
        Application.Quit();
    }
}
