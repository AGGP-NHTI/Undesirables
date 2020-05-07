using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class scrollscript : MonoBehaviour
{

    private void Start()
    {
        StartCoroutine(timer());
    }
    // Update is called once per frame
    void Update()
    {
        gameObject.transform.Translate(Vector2.up * 0.05f);
        if (Input.anyKey)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);//change to load next scene instead of hard coding the second scene
        }
    }

    IEnumerator timer()
    {
        yield return new WaitForSeconds(18f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
