using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApareceChefe : MonoBehaviour
{
    public GameObject chefe;

    void Start()
    {
        chefe.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            chefe.SetActive(true);
        }
    }
}