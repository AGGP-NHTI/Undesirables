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
        /*if(Boss.isDead)
        {
            winCanvas.setActive(true);
        }
        else if(Player.isDead)
        {
            loseCanvas.SetActive(true);
        }*/
    }

    void getScene()
    {
        //SceneManager.GetActiveScene().name;
    }
}
