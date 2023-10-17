using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cacador : Inimigo
{
     public int vida = 3000;
    public int dano = 20;
    private Transform player;
    private Rigidbody2D rb;
    private Animator anim;
    private Vector3 distanciaJogador;
    private bool facingRight = false;
    private bool morto = false;
    private SpriteRenderer sprite;

    //private NightBorneAttack attack1;
    private bool podeAtacar = true;
    private float tempoAtaque;
    private bool comeco = true;
    private int estado = 0;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        //attack1 = GetComponentInChildren<NightBorneAttack>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!morto)
        {
            distanciaJogador = player.transform.position - transform.position;
            if (comeco)
            {
                rb.velocity = new Vector2(3f * (distanciaJogador.x) / Mathf.Abs(distanciaJogador.x), rb.velocity.y);
                anim.SetFloat("velocidade", Mathf.Abs(rb.velocity.x));
                if (Mathf.Abs(distanciaJogador.x) < 2)
                {
                    anim.SetFloat("velocidade", 0f);
                    if (estado == 0)
                    {
                        estado = 1;
                    }
                    else if (estado == 1)
                    {
                        estado = 2;
                    }
                    else if (estado == 2)
                    {
                        estado = 3;
                    }
                    else if (estado == 3)
                    {
                        estado = 1;
                    }
                    comeco = false;
                }

            }
            if (estado == 1 && podeAtacar && !comeco)
            {
                anim.SetTrigger("ataque");
                rb.velocity = new Vector2(3f * (distanciaJogador.x) / Mathf.Abs(distanciaJogador.x), rb.velocity.y);
                //attack1.Blade();
                podeAtacar = false;
                tempoAtaque = Time.time;
            }

            if (estado == 1 && Time.time - tempoAtaque > 1f && !comeco)
            {
                podeAtacar = true;
                comeco = true;
            }

            if (estado == 2 && podeAtacar && !comeco)
            {
                anim.SetTrigger("ataque");
                rb.velocity = new Vector2(3f * (distanciaJogador.x) / Mathf.Abs(distanciaJogador.x), rb.velocity.y);
                //attack1.Blade();
                podeAtacar = false;
                tempoAtaque = Time.time;
            }

            if (estado == 2 && Time.time - tempoAtaque > 1f && !comeco)
            {
                podeAtacar = true;
                comeco = true;
            }

            if (estado == 3 && Time.time - tempoAtaque > 5f && !comeco)
            {
                podeAtacar = true;
                comeco = true;
            }


            float h = (distanciaJogador.x) / Mathf.Abs(distanciaJogador.x);
            if ((h > 0 && !facingRight) || (h < 0 && facingRight))
            {
                Flip();
            }
        }

    }

    public override void Flip()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    public override void Dano(int danoAtual)
    {
        vida -= danoAtual;
        if (vida <= 0)
        {
            morto = true;
            rb.velocity = Vector2.zero;
            anim.SetTrigger("morte");
        }
        else
        {
            StartCoroutine(DanoCoroutine());
        }
    }

    public override  IEnumerator DanoCoroutine()
    {
        rb.velocity = Vector2.zero;
        rb.AddForce(Vector2.right * 8 * (-distanciaJogador.x) / Mathf.Abs(distanciaJogador.x), ForceMode2D.Impulse);
        anim.SetTrigger("Dano");
        for (float i = 0; i < 0.2f; i += 0.2f)
        {
            sprite.color = Color.red;
            yield return new WaitForSeconds(0.3f);
            sprite.color = Color.white;
            yield return new WaitForSeconds(0.5f);
        }
    }

    public void OnCollisionEnter2D(Collision2D other)
    {
        RedHood redhood = other.gameObject.GetComponent<RedHood>();
        if (redhood != null)
        {
            StartCoroutine(ParadoRoutine());
            redhood.Dano(dano);
            redhood.GetComponent<Rigidbody2D>().AddForce(Vector2.right * 7.5f * (distanciaJogador.x) / Mathf.Abs(distanciaJogador.x), ForceMode2D.Impulse);
        }
    }

    public  override IEnumerator ParadoRoutine()
    {
        rb.velocity = Vector2.zero;
        for (float i = 0; i < 0.2f; i += 0.2f)
        {
            yield return new WaitForSeconds(0.8f);
        }
    }

    public override void Morte()
    {
        Destroy(gameObject);
    }
}