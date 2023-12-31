using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Passaro : Inimigo
{
    public AudioSource som;
   public float velocidade = 5;
    public int vida = 150;
    public int dano = 25;

    public GameObject docePrefab;
    private Transform player;
    private Rigidbody2D rb;
    private Animator anim;
    private Vector3 distanciaDoPlayer;
    private bool facingRight = false;
    private bool morto = false;
    private SpriteRenderer sprite;
    private bool move = true;
    private bool ataque = true;
    private bool direcao = true;
    private float tempoAtaque;
    private AtaquePassaro ataquePedra;
    private bool pedra = true;
    private float tempoPedra;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        ataquePedra = GetComponentInChildren<AtaquePassaro>();
        sprite = GetComponent<SpriteRenderer>();
    }

    void FixedUpdate()
    {
        if (!morto && move)
        {
            distanciaDoPlayer = player.transform.position - transform.position;
            if (pedra){
                if (ataquePedra != null){
                    AtaquePassaro pedraAtual = Instantiate(ataquePedra, ataquePedra.transform.position, Quaternion.identity);
                    float directionBall = 1f;
                    //som.Play();
                    anim.SetTrigger("ataque");
                    if (direcao){
                        directionBall = -1f;
                    }
                    if (pedraAtual != null){
                        pedraAtual.Pedra(directionBall);
                    }
                    pedra = false;
                    tempoPedra = Time.time;
                }
            }
            if (!pedra && Time.time - tempoPedra >= 3f){
                pedra = true;
            }

            if (ataque){
                Vector2 direcaoVetor = Vector2.right;
                if (direcao){
                    transform.Translate(direcaoVetor.normalized * -3* Time.deltaTime);
                }
                else{
                    transform.Translate(direcaoVetor.normalized * 3* Time.deltaTime);
                }
                StartCoroutine(AtaqueRoutine());
                tempoAtaque = Time.time;
            }
             if (!ataque && Time.time - tempoAtaque >= 2.5f)
            {
                ataque = true;
                direcao = !direcao;
                Flip();
            }
        }

    }
    public override void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    public override void Dano(int dano)
    {
        vida -= dano;
        if (vida <= 0)
        {
            sprite.color = Color.red;
            morto = true;
            rb.velocity = Vector2.zero;
            anim.SetTrigger("morte");
        }
        else
        {
            StartCoroutine(DanoCoroutine());
        }
    }

    public override IEnumerator DanoCoroutine()
    {
        move = false;
        //rb.velocity = Vector2.zero;
        //anim.SetTrigger("Dano");
        rb.AddForce(Vector2.right * 2 * (-distanciaDoPlayer.x) / Mathf.Abs(distanciaDoPlayer.x), ForceMode2D.Impulse);
        for (float i = 0; i<0.2f; i += 0.2f)
        {
            sprite.color = Color.red;
            yield return new WaitForSeconds(0.3f);
            sprite.color = Color.white;
            yield return new WaitForSeconds(0.3f);
        }
        move = true;
    }

    public IEnumerator AtaqueRoutine()
    {
        for (float i = 0; i < 0.2f; i += 0.2f)
        {
            yield return new WaitForSeconds(2.3f);
        }
        ataque = false;
    }

    public void OnCollisionEnter2D(Collision2D other)
    {
        RedHood redhood = other.gameObject.GetComponent<RedHood>();
        if (redhood != null)
        {
            StartCoroutine(ParadoRoutine());
            redhood.Dano(dano);
            redhood.GetComponent<Rigidbody2D>().AddForce(Vector2.right * 2 * (distanciaDoPlayer.x) / Mathf.Abs(distanciaDoPlayer.x), ForceMode2D.Impulse);
        }  
    }

    public override IEnumerator ParadoRoutine()
    {
        move = false;
        rb.velocity = Vector2.zero;
        //anim.SetFloat("velocidade", Mathf.Abs(rb.velocity.x));
        for (float i = 0; i < 0.2f; i += 0.2f)
        {
            yield return new WaitForSeconds(0.8f);
        }
        move = true;
    }
    public override void Morte()
    {
        Instantiate(docePrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
