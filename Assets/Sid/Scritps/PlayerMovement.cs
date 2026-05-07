using UnityEngine;

[RequireComponent(typeof(PlayerEntity))]
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    private PlayerEntity atributos; // Acesso à vida e status
    private Rigidbody2D rb;
    private Animator anim;
    
    private float tempoPressionado = 0f;
    private float cronometroInatividade = 0f;

    void Start()
    {
        atributos = GetComponent<PlayerEntity>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        // Se estiver morto, ignora todos os comandos
        if (atributos.vidaAtual <= 0) return;

        float movX = Input.GetAxisRaw("Horizontal");
        bool apertouPulo = Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow);

        // 1. VERIFICAÇÃO DE ATIVIDADE
        if (movX != 0 || apertouPulo)
        {
            cronometroInatividade = 0f; 
        }
        else
        {
            cronometroInatividade += Time.deltaTime;
            // Lê o tempo de inatividade configurado no PlayerEntity
            if (cronometroInatividade >= atributos.tempoParaEntrarIdle)
            {
                anim.SetTrigger("playIdle"); 
                cronometroInatividade = 0f;  
            }
        }

        // 2. LÓGICA DE MOVIMENTO E ROTAÇÃO
        float velAtual = 0f;
        if (movX != 0) 
        {
            tempoPressionado += Time.deltaTime;
            
            // Lê o tempo de corrida configurado no PlayerEntity
            if (tempoPressionado >= atributos.tempoParaCorrer) 
            {
                velAtual = atributos.velocidadeCorrer; 
                anim.SetFloat("Speed", 2.5f); 
            } 
            else 
            {
                velAtual = atributos.velocidadeAndar; 
                anim.SetFloat("Speed", 1.0f); 
            }

            if (movX > 0) transform.eulerAngles = new Vector3(0, 0, 0); 
            else if (movX < 0) transform.eulerAngles = new Vector3(0, 180f, 0); 
        }
        else 
        {
            tempoPressionado = 0f; 
            anim.SetFloat("Speed", 0f); 
        }

        rb.linearVelocity = new Vector2(movX * velAtual, rb.linearVelocity.y);

        // 3. LÓGICA DE PULO ORIGINAL
        if (apertouPulo && !anim.GetBool("isJumping"))
        {
            // Lê a força de pulo configurada no PlayerEntity
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, atributos.forcaPulo);
            anim.SetBool("isJumping", true); 
        }
    }

    // 4. VERIFICAÇÃO DE CHÃO ORIGINAL
    private void OnCollisionEnter2D(Collision2D collision)
    {
        anim.SetBool("isJumping", false);
    }
}