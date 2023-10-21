using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtaqueEspada : MonoBehaviour
{
    private Animator anim;
    private int damage = 75;
    public Vector2 direction = Vector2.right;
    private float startTime;
    void Start(){}

    // Update is called once per frame
    void FixedUpdate()
    {
        // Check if the fire attack has been active for 5 seconds and destroy it.
        if (Time.time - startTime >= 2.1f)
        {
        }
        
    }

    public void Espada()
    {
        anim = GetComponent<Animator>();
        startTime = Time.time;
        anim.Play("espada");
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        Inimigo inimigo = other.GetComponent<Inimigo>();
        if (inimigo != null)
        {
            inimigo.Dano(damage);
        }

    }
}
