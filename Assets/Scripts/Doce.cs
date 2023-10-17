using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Doce : MonoBehaviour
{
    // Start is called before the first frame update
    private float tempo;
    private bool direcao = false;
    void Start()
    {
        tempo = Time.time;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 direcaoVetor = Vector2.up;
        if (Time.time - tempo > 0.3f && direcao){
            transform.localScale = new Vector3(0.15f,0.15f,0f);
            //transform.Translate(direcaoVetor.normalized * -5* Time.deltaTime);
            tempo = Time.time;
            direcao = false;
        }
        else if (Time.time - tempo > 0.3f && !direcao){
            transform.localScale = new Vector3(0.1f,0.1f,0f);
            //transform.Translate(direcaoVetor.normalized * 5* Time.deltaTime);
            tempo = Time.time;
            direcao = true;
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        RedHood redhood = other.GetComponent<RedHood>();
        if (redhood != null)
        {
            Destroy(gameObject);
            //redhood.Dano(dano);
        }

    }

}
