using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class MenuScript : MonoBehaviour
{
    public GameObject mainMenuPanel;
    public GameObject optionsPanel;
    public GameObject howToPanel;
    public AudioMixer audioMixer;
    string mainMenuScene = "MainMenu";

    public static MenuScript instance;

    public GameObject MainMenuObject;

    private void Awake()
    {


        if (instance == null)
        {
            instance = this;
            SceneManager.sceneLoaded += OnSceneLoaded;
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


    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        MainMenuObject.SetActive(false);
    }

    public void EnterOptions()
    {
        mainMenuPanel.SetActive(false);
        optionsPanel.SetActive(true);
    }

    public void EnterHowTo()
    {
        mainMenuPanel.SetActive(false);
        howToPanel.SetActive(true);
    }

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("MasterVolume", volume);
        Debug.Log(volume);
    }

    public void BackButton()
    {
        mainMenuPanel.SetActive(true);
        optionsPanel.SetActive(false);
        howToPanel.SetActive(false);
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == mainMenuScene)
        {
            MainMenuObject.SetActive(true);
        }
    }

    public void QuitGame()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif

    }

}

