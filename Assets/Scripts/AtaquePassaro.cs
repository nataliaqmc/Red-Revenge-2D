using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtaquePassaro : MonoBehaviour
{
 
    private Animator anim;
    private int damage = 40;
    private bool active = false;
    public Vector2 direction = Vector2.right;
    private float startTime;

    void Start(){

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (active)
        {
            float distance = 7 * (Time.time - startTime);

            // Move the fire attack in the specified direction.
            transform.Translate(direction.normalized * distance * Time.deltaTime);

            // Check if the fire attack has been active for 5 seconds and destroy it.
            if (Time.time - startTime >= 2f)
            {
                active = false;
                Destroy(gameObject);
            }
        }
    }

    public void Pedra(float attackDirection)
    {
        //transform.localScale = new Vector3(1f, 1f, 1f);
        if (attackDirection == 1f)
        {
            direction = Vector2.right;
        }
        else
        {
            direction = Vector2.left;
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }
        anim = GetComponent<Animator>();
        startTime = Time.time;
        anim.Play("Pedra");
        active = true;
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        RedHood inimigo = other.GetComponent<RedHood>();
        if (inimigo != null)
        {
            inimigo.Dano(damage);
            Destroy(gameObject);
        }

    }
}
