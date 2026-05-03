using UnityEngine;
using UnityEngine.UI; 
using TMPro; // NOVO: Biblioteca obrigatória para usar o TextMeshPro!

public class PlayerMovement : MonoBehaviour
{
    [Header("Configurações de Movimento")]
    public float velocidadeAndar = 3f;
    public float velocidadeCorrer = 6f;
    public float forcaPulo = 7f;
    public float tempoParaCorrer = 0.5f;
    
    [Header("Combate e Tiro")]
    public Transform firePoint;
    public GameObject projectilePrefab;

    [Header("Configurações de Inatividade")]
    public float tempoParaEntrarIdle = 3.0f; 
    private float cronometroInatividade = 0f;

    [Header("Saúde e HUD")]
    public float vidaMaxima = 100f;
    public float vidaAtual;
    public Image barraDeVida; 

    [Header("Sistema de Poções")]
    public int pocoesAtuais = 0;
    public float valorDeCura = 30f;
    public TextMeshProUGUI textoContadorPocoes; // NOVO: Variável atualizada para o TextMeshPro!

    // Componentes internos
    private Rigidbody2D rb;
    private Animator anim;
    private float tempoPressionado = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        
        vidaAtual = vidaMaxima; 
        AtualizarHUD(); 
    }

    void Update()
    {
        if (vidaAtual <= 0) return;

        float movX = Input.GetAxisRaw("Horizontal");
        bool apertouPulo = Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow);
        bool apertouAtaqueCurto = Input.GetKeyDown(KeyCode.Z);
        bool apertouAtaqueLongo = Input.GetKeyDown(KeyCode.X);
        bool apertouPocao = Input.GetKeyDown(KeyCode.E); 

        // 1. VERIFICAÇÃO DE ATIVIDADE
        if (movX != 0 || apertouPulo || apertouAtaqueCurto || apertouAtaqueLongo || apertouPocao)
        {
            cronometroInatividade = 0f; 
        }
        else
        {
            cronometroInatividade += Time.deltaTime;
            if (cronometroInatividade >= tempoParaEntrarIdle)
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

            if (movX > 0) transform.eulerAngles = new Vector3(0, 0, 0); 
            else if (movX < 0) transform.eulerAngles = new Vector3(0, 180f, 0); 
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

        // 4. ATAQUES E AÇÕES
        if (apertouAtaqueCurto) anim.SetTrigger("attackShort");
        if (apertouAtaqueLongo) anim.SetTrigger("attackLong");
        if (apertouPocao) UsarPocao();
        
        // 5. TESTE DE DANO 
        if (Input.GetKeyDown(KeyCode.T))
        {
            TomarDano(10f);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        anim.SetBool("isJumping", false);
    }
    
    public void Shoot()
    {
        Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
    }

    // ----------------------------------------------------
    // SISTEMAS DE VIDA E HUD
    // ----------------------------------------------------

    public void TomarDano(float dano)
    {
        vidaAtual -= dano;

        if (vidaAtual <= 0)
        {
            vidaAtual = 0; 
            AtualizarHUD();
            Morrer();
        }
        else
        {
            AtualizarHUD();
        }
    }

    private void Morrer()
    {
        anim.SetTrigger("die");
        rb.linearVelocity = Vector2.zero; 
        this.enabled = false; 
    }

    public void AdicionarPocao(int quantidade)
    {
        pocoesAtuais += quantidade;
        AtualizarHUD();
    }

    private void UsarPocao()
    {
        if (pocoesAtuais > 0 && vidaAtual < vidaMaxima)
        {
            pocoesAtuais--; 
            vidaAtual += valorDeCura; 

            if (vidaAtual > vidaMaxima)
            {
                vidaAtual = vidaMaxima;
            }

            AtualizarHUD();
        }
    }

    private void AtualizarHUD()
    {
        if (barraDeVida != null)
        {
            barraDeVida.fillAmount = vidaAtual / vidaMaxima;
        }

        if (textoContadorPocoes != null)
        {
            textoContadorPocoes.text = pocoesAtuais.ToString();
        }
    }
}