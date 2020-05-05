using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseScript : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public static PauseScript instance;

    public GameObject pauseMenuObject;
    public GameObject pauseMenuCanvas;
    public GameObject optionsMenuCanvas;

    

    private void Awake()
    {
       
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
       if( Input.GetKeyDown(KeyCode.Escape))
        {
            if(GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    string getScene()
    {
        return SceneManager.GetActiveScene().name;
    }

    public void returntoMainMenu()
    {
        SceneManager.LoadScene("MainMenu");

        Resume();
    }

    public void Resume()
    {
        pauseMenuObject.SetActive(false);

        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    void Pause()
    {
        if(getScene() != "MainMenu" && getScene() != "Scroll")
        {
            pauseMenuObject.SetActive(true);

            Time.timeScale = 0f;
            GameIsPaused = true;
        }
        
    }

    public void EnterOptions()
    {
        pauseMenuCanvas.SetActive(false);
        optionsMenuCanvas.SetActive(true);
    }

    public void ExitOptions()
    {

        pauseMenuCanvas.SetActive(true);
        optionsMenuCanvas.SetActive(false);
    }

    
}
