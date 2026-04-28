using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float velocidadeAndar = 3f;
    public float velocidadeCorrer = 6f;
    public float forcaPulo = 7f;
    
    public float tempoParaCorrer = 0.5f; 

    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sprite;
    
    private float tempoPressionado = 0f; 

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // 1. MOVIMENTO E CORRIDA POR TEMPO
        float movX = Input.GetAxisRaw("Horizontal"); // A/D ou Setas
        float velAtual = 0f;

        if (movX != 0) 
        {
            tempoPressionado += Time.deltaTime;

            if (tempoPressionado >= tempoParaCorrer) 
            {
                velAtual = velocidadeCorrer;
                anim.SetFloat("Speed", 2.5f); 
            } 
            else 
            {
                velAtual = velocidadeAndar;
                anim.SetFloat("Speed", 1.0f); 
            }

            sprite.flipX = movX < 0; 
        }
        else 
        {
            tempoPressionado = 0f; 
            anim.SetFloat("Speed", 0f); 
        }

        rb.linearVelocity = new Vector2(movX * velAtual, rb.linearVelocity.y);

        // 2. PULO (Tecla W ou Seta para Cima)
        if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) && !anim.GetBool("isJumping"))
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, forcaPulo);
            anim.SetBool("isJumping", true); 
        }

        // 3. ATAQUES E MORTE (Teclas Z, X, C)
        if (Input.GetKeyDown(KeyCode.Z)) anim.SetTrigger("attackShort");
        if (Input.GetKeyDown(KeyCode.X)) anim.SetTrigger("attackLong");
        if (Input.GetKeyDown(KeyCode.C)) anim.SetTrigger("die");
    }

    // 4. DETECTAR O CHÃO
    private void OnCollisionEnter2D(Collision2D collision)
    {
        anim.SetBool("isJumping", false);
    }
}