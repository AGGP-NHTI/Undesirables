using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject Boss;
    public GameObject Player;

    public GameObject winCanvas;
    public GameObject loseCanvas;

    void Start()
    {
        
    }

    
    void Update()
    {
        if (Boss.GetComponent<BossPawn>().isDead)
        {
            winCanvas.SetActive(true);
        }
        else if(Player.GetComponent<HeroPawn>().isDead)
        {
            loseCanvas.SetActive(true);
        }
    }

    public void returnToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void reloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
