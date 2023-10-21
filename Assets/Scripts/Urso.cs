using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Urso : Inimigo
{
   public float velocidade = 3;
   public AudioSource som;
    public int vida = 300;
    public int dano = 50;

    public GameObject docePrefab;
    private Transform player;
    private Rigidbody2D rb;
    private Animator anim;
    private Vector3 distanciaDoPlayer;
    private bool facingRight = false;
    private bool morto = false;
    private SpriteRenderer sprite;
    private bool move = true;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
    }

    void FixedUpdate()
    {
        if (!morto && move)
        {
            distanciaDoPlayer = player.transform.position - transform.position;
            if (Mathf.Abs(distanciaDoPlayer.x) < 12 && 
            Mathf.Abs(distanciaDoPlayer.x) > 1.5f &&
             Mathf.Abs(distanciaDoPlayer.y) < 5)
            {
                rb.velocity = new Vector2(velocidade * (distanciaDoPlayer.x) / Mathf.Abs(distanciaDoPlayer.x), rb.velocity.y);
                anim.SetFloat("velocidade", Mathf.Abs(rb.velocity.x));
            }
            if (Mathf.Abs(distanciaDoPlayer.x) < 1.5f)
            {
                rb.velocity = new Vector2(velocidade * (distanciaDoPlayer.x) / (1.5f*Mathf.Abs(distanciaDoPlayer.x)), rb.velocity.y);
                anim.SetTrigger("Ataque");
                som.Play();
            }
            float h = rb.velocity.x;
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

    public override void Dano(int dano)
    {
        vida -= dano;
        if (vida <= 0)
        {
            sprite.color = Color.red;
            morto = true;
            rb.velocity = Vector2.zero;
            anim.SetTrigger("Morte");
        }
        else
        {
            StartCoroutine(DanoCoroutine());
        }
    }

    public override IEnumerator DanoCoroutine()
    {
        move = false;
        rb.velocity = Vector2.zero;
        anim.SetTrigger("Dano");
        rb.AddForce(Vector2.right * 5 * (-distanciaDoPlayer.x) / Mathf.Abs(distanciaDoPlayer.x), ForceMode2D.Impulse);
        for (float i = 0; i<0.2f; i += 0.2f)
        {
            sprite.color = Color.red;
            yield return new WaitForSeconds(0.3f);
            sprite.color = Color.white;
            yield return new WaitForSeconds(0.3f);
        }
        move = true;
    }

    public void OnCollisionEnter2D(Collision2D other)
    {
        RedHood redhood = other.gameObject.GetComponent<RedHood>();
        if (redhood != null)
        {
            StartCoroutine(ParadoRoutine());
            redhood.Dano(dano);
            redhood.GetComponent<Rigidbody2D>().AddForce(Vector2.right * 5 * (distanciaDoPlayer.x) / Mathf.Abs(distanciaDoPlayer.x), ForceMode2D.Impulse);
        }  
    }

    public override IEnumerator ParadoRoutine()
    {
        move = false;
        rb.velocity = Vector2.zero;
        anim.SetFloat("velocidade", Mathf.Abs(rb.velocity.x));
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
