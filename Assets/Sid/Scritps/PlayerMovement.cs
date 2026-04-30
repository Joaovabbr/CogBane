using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Configurações de Movimento")]
    public float velocidadeAndar = 3f;
    public float velocidadeCorrer = 6f;
    public float forcaPulo = 7f;
    public float tempoParaCorrer = 0.5f;

    [Header("Configurações de Inatividade")]
    public float tempoParaEntrarIdle = 3.0f; // A cada 3 segundos ele toca a animação
    private float cronometroInatividade = 0f;

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
        float movX = Input.GetAxisRaw("Horizontal");
        bool apertouPulo = Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow);
        bool apertouAtaque = Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.X);

        // 1. VERIFICAÇÃO DE ATIVIDADE (O CICLO DE 3 SEGUNDOS)
        if (movX != 0 || apertouPulo || apertouAtaque)
        {
            cronometroInatividade = 0f; // Reseta se fizer qualquer ação
        }
        else
        {
            cronometroInatividade += Time.deltaTime;

            if (cronometroInatividade >= tempoParaEntrarIdle)
            {
                anim.SetTrigger("playIdle"); // Dispara o "suspiro" uma vez
                cronometroInatividade = 0f;  // Reseta o cronômetro para o próximo ciclo
            }
        }

        // 2. LÓGICA DE MOVIMENTO
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

        // 3. LÓGICA DE PULO
        if (apertouPulo && !anim.GetBool("isJumping"))
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, forcaPulo);
            anim.SetBool("isJumping", true); 
        }

        // 4. ATAQUES E MORTE
        if (Input.GetKeyDown(KeyCode.Z)) anim.SetTrigger("attackShort");
        if (Input.GetKeyDown(KeyCode.X)) anim.SetTrigger("attackLong");
        if (Input.GetKeyDown(KeyCode.C)) anim.SetTrigger("die");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        anim.SetBool("isJumping", false);
    }
}