using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinTransition : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        StartCoroutine(timer());
    }

    

    IEnumerator timer()
    {
        yield return new WaitForSeconds(8f);
        SceneManager.LoadScene("MainMenu");
    }
}
