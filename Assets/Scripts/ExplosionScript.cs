using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionScript : MonoBehaviour
{
    AudioSource boom;
    void Start()
    {
        boom = GetComponent<AudioSource>();
        boom.Play();
        Destroy(gameObject, 1f);
    }

}
