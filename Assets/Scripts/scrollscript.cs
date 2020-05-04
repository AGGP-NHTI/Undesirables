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
        gameObject.transform.Translate(Vector2.up * 0.004f);
        if (Input.anyKey)
        {
            SceneManager.LoadScene(1);
        }
    }

    IEnumerator timer()
    {
        yield return new WaitForSeconds(35f);
        SceneManager.LoadScene(1);
    }
}
