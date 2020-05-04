using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    bool isInMotion;
    bool goingUp;
    Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        goingUp = false;
        isInMotion = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isInMotion)
        {
            if (goingUp == true)
            {
                StartCoroutine(up());
            }
            else
            {
                StartCoroutine(down());
            }
        }
    }

    IEnumerator up()
    {
        isInMotion = true;
        for (int i = 0; i < 20; i++)
        {
            transform.Translate(Vector2.up * 2.0f);
        }
        isInMotion = false;
        goingUp = false;
        yield return null;
    }
    IEnumerator down()
    {
        isInMotion = true;
        for (int i = 0; i < 20; i++)
        {
            transform.Translate(Vector2.up * -2.0f);
        }
        isInMotion = false;
        goingUp = true;
        yield return null;
    }
}
