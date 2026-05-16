using UnityEngine;

[RequireComponent(typeof(PlayerEntity))]
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Áudio")]
    public AudioSource audioSource; // Arraste o Damon para cá
    public AudioClip somPouso;      // Arraste o som de pouso aqui

    private PlayerEntity atributos; 
    private Rigidbody2D rb;
    private Animator anim;
    private Collider2D col; 

    private float tempoPressionado = 0f;
    private float cronometroInatividade = 0f;

    public PlayerCombat playerCombat;

    [Header("Configurações de Chão")]
    public LayerMask groundLayer; 
    private bool isGrounded;
    private bool wasGrounded = true; 
    private float distanceToGround;

    void Start()
    {
        atributos = GetComponent<PlayerEntity>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        col = GetComponent<Collider2D>(); 
        distanceToGround = col.bounds.extents.y;

        if (audioSource == null) audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (atributos.vidaAtual <= 0) return;

     
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, distanceToGround + 0.02f, groundLayer);

        if (!wasGrounded && isGrounded && rb.linearVelocity.y <= 0)
        {
            if (audioSource != null && somPouso != null)
            {
                audioSource.pitch = Random.Range(0.9f, 1.1f);
                audioSource.PlayOneShot(somPouso);
            }
        }
        
        wasGrounded = isGrounded; 


        anim.SetBool("isJumping", !isGrounded);

        if (playerCombat != null && playerCombat.isAttacking)
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            anim.SetFloat("Speed", 0f); 
            cronometroInatividade = 0f; 
            return; 
        }

        float movX = Input.GetAxisRaw("Horizontal");
        bool apertouPulo = Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow);

        if (movX != 0 || apertouPulo)
        {
            cronometroInatividade = 0f; 
        }
        else
        {
            cronometroInatividade += Time.deltaTime;
            if (cronometroInatividade >= atributos.tempoParaEntrarIdle)
            {
                anim.SetTrigger("playIdle"); 
                cronometroInatividade = 0f;  
            }
        }

        float velAtual = 0f;
        if (movX != 0) 
        {
            tempoPressionado += Time.deltaTime;
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
                if (playerCombat != null) playerCombat.isFlipped = false;
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

        if (apertouPulo && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, atributos.forcaPulo);
            audioSource.pitch = 1f; 
        }
    }
}