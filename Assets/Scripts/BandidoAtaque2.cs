using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BandidoAtaque2 :  MonoBehaviour
{
      private Animator anim;
    private int dano = 60;
    public Vector2 direcao = Vector2.right;
    private float comeco;
    void Start(){}

    // Update is called once per frame
    void FixedUpdate()
    {
        // Check if the fire attack has been active for 5 seconds and destroy it.
        if (Time.time - comeco >= 2.1f)
        {
        }
        
    }

    public void Machado()
    {
        anim = GetComponent<Animator>();
        comeco = Time.time;
        anim.Play("Ataque20");
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        RedHood redhood = other.GetComponent<RedHood>();
        if (redhood != null)
        {
            redhood.Dano(dano);
        }

    }
}
