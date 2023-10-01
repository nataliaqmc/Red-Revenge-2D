using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtaqueMachado : MonoBehaviour
{
    private Animator anim;
    private int damage = 120;
    public Vector2 direction = Vector2.right;
    private float startTime;
    void Start(){}

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Time.time - startTime >= 2.2f)
        {
            //Destroy(gameObject);
        }
    }

    public void Machado()
    {
        anim = GetComponent<Animator>();
        startTime = Time.time;
        anim.Play("machado");
    }
}
