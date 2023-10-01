using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedHood : MonoBehaviour
{
    // Start is called before the first frame update
    public float maxSpeed = 5;
    public Transform chao;
    public float jumpForce;

    private Rigidbody2D rb;
    private float speed;
    private bool facingRight = true;
    private bool onGround;
    private bool jump = false;
    private bool doubleJump;
    private Animator anim;

    //Ataque1
    private bool ataque1 = true;
    private bool podeAtacar1 = false;
    private float ultimoAtaque1;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        speed = maxSpeed;
        anim = GetComponent<Animator>();
        
    }

    // Update is called once per frame
    void Update()
    {
        onGround = Physics2D.Linecast(transform.position, chao.position, 1 << LayerMask.NameToLayer("Chao"));
        if (onGround)
        {
            anim.SetTrigger("chao");
            doubleJump = false;
        }


        if (Input.GetButtonDown("Jump") && (onGround || !doubleJump))
        {
            jump = true;
            if (!doubleJump && !onGround)
            {
                doubleJump = true;
            }
        }

         if (ataque1 && Input.GetKeyDown(KeyCode.Z))
        {
            ataque1 = false;
            podeAtacar1 = true;
            ultimoAtaque1 = Time.time;
        }
        if (!ataque1 && Time.time - ultimoAtaque1 >= 0.6f)
        {
            ataque1 = true;
        }
    }

    private void FixedUpdate()
    {
        float h = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(h * speed, rb.velocity.y);
        anim.SetFloat("velocidade", Mathf.Abs(h));
        if ((h > 0 && !facingRight) || (h < 0 && facingRight))
        {
            Flip();
        }

        if (podeAtacar1){
            rb.velocity = Vector2.zero;
            anim.SetTrigger("ataque1");
            podeAtacar1 = false;
        }

        if (jump)
        {
            anim.SetTrigger("salto");
            rb.velocity = Vector2.zero;
            rb.AddForce(Vector2.up * jumpForce);
            jump = false;
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
}