using UnityEngine;

[RequireComponent(typeof(PlayerEntity))]
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    private PlayerEntity atributos; 
    private Rigidbody2D rb;
    private Animator anim;
    
    // NOVO: Colisor necessário para calcular a distância até o pé
    private Collider2D col; 

    private float tempoPressionado = 0f;
    private float cronometroInatividade = 0f;

    public PlayerCombat playerCombat;

    [Header("Configurações de Chão")]
    public LayerMask groundLayer; // Layer que representa o chão
    private bool isGrounded;
    private float distanceToGround;

    void Start()
    {
        atributos = GetComponent<PlayerEntity>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        
        // NOVO: Pega o colisor e calcula a distância do centro até o pé do player
        col = GetComponent<Collider2D>(); 
        distanceToGround = col.bounds.extents.y;
    }

    void Update()
    {
        // Se estiver morto, ignora todos os comandos
        if (atributos.vidaAtual <= 0) return;

        // NOVO: Lógica de Raycast baseada na imagem para detectar se está no chão
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, distanceToGround + 0.02f, groundLayer);
        
        // NOVO: O Animator agora é atualizado automaticamente baseado no Raycast
        anim.SetBool("isJumping", !isGrounded);

        float movX = Input.GetAxisRaw("Horizontal");
        
        // ATUALIZADO: Aceita tanto W quanto Espaço para pular
        bool apertouPulo = Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space);

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

            if (movX > 0)
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
            } 
            else if (movX < 0)
            {
                transform.eulerAngles = new Vector3(0, 180f, 0);
                if (playerCombat != null) playerCombat.isFlipped = true;
            } 
        }
        else 
        {
            tempoPressionado = 0f; 
            anim.SetFloat("Speed", 0f); 
        }

        rb.linearVelocity = new Vector2(movX * velAtual, rb.linearVelocity.y);

        // 3. LÓGICA DE PULO (Atualizada com a lógica de Grounded da imagem)
        if (apertouPulo && isGrounded)
        {
            // Lê a força de pulo configurada no PlayerEntity
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, atributos.forcaPulo);
        }
    }
}