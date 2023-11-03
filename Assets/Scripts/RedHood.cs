using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
public class RedHood : MonoBehaviour
{
    // Start is called before the first frame update

    public FixedJoystick analogico;
    public AudioSource somespada;
    public AudioSource upgrade;
    public AudioSource doce;
    public AudioSource dano;
    public AudioSource somflecha;
    //public AudioSource somespada;
    public float maxSpeed = 3.75f;
    public Transform chao;
    public float jumpForce;
    public int vidaMaxima = 700;
    public static int doces = 0;

    public TextMeshProUGUI QuantidadeDoces;

    private int vida;

    public Image Barfile;
    private Rigidbody2D rb;
    public static float speed;
    private bool facingRight = true;
    private bool onGround;
    private bool jump = false;
    private bool move = false;
    private bool doubleJump;
    private Animator anim;

    //Ataque1
    private bool ataque1 = true;
    private bool podeAtacar1 = false;
    private float ultimoAtaque1;
    private AtaqueEspada espada;

    //Ataque2
    private bool ataque2 = true;
    private bool podeAtacar2 = false;
    private float ultimoAtaque2;
    private AtaqueMachado machado;

    private SpriteRenderer sprite;

    //Ataque3
    private bool ataque3 = true;
    private bool podeAtacar3 = false;
    private float ultimoAtaque3;
    private AtaqueFlecha flecha;
    private int direcaoFlecha = 1;

    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        QuantidadeDoces.text = doces.ToString();
        vida = vidaMaxima;
        rb = GetComponent<Rigidbody2D>();
        speed = maxSpeed;
        anim = GetComponent<Animator>();
        espada = GetComponentInChildren<AtaqueEspada>();
        machado = GetComponentInChildren<AtaqueMachado>();
        flecha = GetComponentInChildren<AtaqueFlecha>();   
    }

    // Update is called once per frame
    void Update()
    {
        onGround = Physics2D.Linecast(transform.position, chao.position, 1 << LayerMask.NameToLayer("Chao"));

        if (transform.position.y < -10){
            doces = 0;
            QuantidadeDoces.text = doces.ToString();
            string nomeDaCenaAtual = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene(nomeDaCenaAtual);
        }
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

        if (doces >= 5 && Input.GetKeyDown(KeyCode.T)){
            upgrade.Play();
            doces -= 5;
            QuantidadeDoces.text = doces.ToString();
            StartCoroutine(AumentaVida());
            vida += 100;
            if (vida >= 700){
                vida = 700;
            }
            Barfile.fillAmount = (float) vida/vidaMaxima;
        }

        if (doces >= 5 && Input.GetKeyDown(KeyCode.U)){
            upgrade.Play();
            doces -= 5;
            QuantidadeDoces.text = doces.ToString();
            speed+=0.75f;
            if (speed >= 10){
                speed = 10;
            }
            StartCoroutine(Aumentavelocidade());
        }

        if (ataque1 && Input.GetKeyDown(KeyCode.Z))
        {
            somespada.Play();
            ataque1 = false;
            podeAtacar1 = true;
            ultimoAtaque1 = Time.time;
            move = true;
        }
        if (!ataque1 && Time.time - ultimoAtaque1 >= 0.6f)
        {
            ataque1 = true;
            move = false;
        }

        if (ataque2 && Input.GetKeyDown(KeyCode.X))
        {
            ataque2 = false;
            somespada.Play();
            podeAtacar2 = true;
            ultimoAtaque2 = Time.time;
            move = true;
        }
        if (!ataque2 && Time.time - ultimoAtaque2 >= 1.5f)
        {
            move = false;
        }

        if (!ataque2 && Time.time - ultimoAtaque2 >= 3f)
        {
            ataque2 = true;
        }

        if (ataque3 && Input.GetKeyDown(KeyCode.C))
        {
            somflecha.Play();
            ataque3 = false;
            podeAtacar3 = true;
            ultimoAtaque3 = Time.time;
            move = true;
        }
        if (!ataque3 && Time.time - ultimoAtaque3 >= 0.8f)
        {
            ataque3 = true;
            move = false;
        }
    }

    private void FixedUpdate()
    {
        //float h = Input.GetAxisRaw("Horizontal");
        float h = analogico.Horizontal;
        rb.velocity = new Vector2(h * speed, rb.velocity.y);
        anim.SetFloat("velocidade", Mathf.Abs(h));
        if ((h > 0 && !facingRight) || (h < 0 && facingRight))
        {
            Flip();
        }

        if (move){
            rb.velocity = new Vector2(0, rb.velocity.y);
        }

        if (podeAtacar1){
            espada.Espada();
            rb.velocity = Vector2.zero;
            anim.SetTrigger("ataque1");
            podeAtacar1 = false;
        }

        if (podeAtacar2){
            machado.Machado();
            rb.velocity = Vector2.zero;
            anim.SetTrigger("ataque2");
            podeAtacar2 = false;
        }

        if (podeAtacar3){
            AtaqueFlecha novaFlecha = Instantiate(flecha, flecha.transform.position, Quaternion.identity);
            novaFlecha.Flecha(direcaoFlecha);
            rb.velocity = Vector2.zero;
            anim.SetTrigger("ataque3");
            podeAtacar3 = false;
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
        direcaoFlecha *=-1;
    }

    public   IEnumerator MorteCoroutine()
    {
        for (float i = 0; i < 0.2f; i += 0.2f)
        {
            yield return new WaitForSeconds(1f);
            string nomeDaCenaAtual = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene(nomeDaCenaAtual);
        }
    }

        public IEnumerator AumentaVida(){
            for (float i = 0; i < 0.2f; i += 0.2f)
        {
            sprite.color = Color.yellow;
            yield return new WaitForSeconds(1f);
            sprite.color = Color.white;
            
        }
    }

    public IEnumerator Aumentavelocidade(){
            for (float i = 0; i < 0.2f; i += 0.2f)
        {
            sprite.color = Color.blue;
            yield return new WaitForSeconds(1f);
            sprite.color = Color.white;
            
        }
    }

    public void ColetaDoce(){
        doce.Play();
        doces+= 1;
        QuantidadeDoces.text = doces.ToString();
    }

    public void Dano(int damage)
    {
        vida -= damage;
        Barfile.fillAmount = (float) vida/vidaMaxima;
        dano.Play();
        if (vida <= 0)
        {
            anim.SetTrigger("morte");
            doces = 0;
            QuantidadeDoces.text = doces.ToString();
            StartCoroutine(MorteCoroutine());
        }
        else
        {
            //rb.AddForce(Vector2.right * 5 * direction, ForceMode2D.Impulse);
            anim.SetTrigger("dano");
        }
    }
}