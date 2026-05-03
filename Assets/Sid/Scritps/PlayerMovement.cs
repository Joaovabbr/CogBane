using UnityEngine;
using UnityEngine.UI; // NOVO: Obrigatório para mexer com a barrinha do HUD!

public class PlayerMovement : MonoBehaviour
{
    [Header("Configurações de Movimento")]
    public float velocidadeAndar = 3f;
    public float velocidadeCorrer = 6f;
    public float forcaPulo = 7f;
    public float tempoParaCorrer = 0.5f;
    public Transform firePoint;
    public GameObject projectilePrefab;

    [Header("Configurações de Inatividade")]
    public float tempoParaEntrarIdle = 3.0f; 
    private float cronometroInatividade = 0f;

    [Header("Saúde e HUD")] // NOVO: Sessão da vida
    public float vidaMaxima = 100f;
    public float vidaAtual;
    public Image barraDeVida; // A caixinha onde você vai arrastar o Fill_Sangue

    private Rigidbody2D rb;
    private Animator anim;
    private float tempoPressionado = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        
        // NOVO: Enche a vida quando o jogo começa
        vidaAtual = vidaMaxima; 
        if(barraDeVida != null) barraDeVida.fillAmount = 1f;
    }

    void Update()
    {
        // Se já morreu, ignora os inputs e sai do Update
        if (vidaAtual <= 0) return;

        float movX = Input.GetAxisRaw("Horizontal");
        bool apertouPulo = Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow);
        bool apertouAtaque = Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.X);

        // 1. VERIFICAÇÃO DE ATIVIDADE
        if (movX != 0 || apertouPulo || apertouAtaque)
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

        // 2. LÓGICA DE MOVIMENTO (com a rotação 3D)
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

        // 4. ATAQUES
        if (Input.GetKeyDown(KeyCode.Z)) anim.SetTrigger("attackShort");
        if (Input.GetKeyDown(KeyCode.X)) anim.SetTrigger("attackLong");
        
        // 5. TESTE DE DANO (NOVO: Aperte T para tomar 10 de dano)
        if (Input.GetKeyDown(KeyCode.T))
        {
            TomarDano(10f);
        }
        
        // A lógica de apertar 'C' para morrer foi removida, 
        // agora o personagem morre sozinho se a vida zerar!
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        anim.SetBool("isJumping", false);
    }
    
    public void Shoot()
    {
        Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
    }

    // NOVO: Função chamada sempre que o jogador apanha
    public void TomarDano(float dano)
    {
        vidaAtual -= dano;

        // Atualiza a barrinha visual (a conta resulta em algo entre 0.0 e 1.0)
        if (barraDeVida != null)
        {
            barraDeVida.fillAmount = vidaAtual / vidaMaxima;
        }

        // Checa se o golpe foi fatal
        if (vidaAtual <= 0)
        {
            vidaAtual = 0; // Trava no zero para a barrinha não dar erro
            Morrer();
        }
        else
        {
            // Opcional para o futuro: Tocar animação de Hit
            // anim.SetTrigger("takeHit"); 
        }
    }

    // NOVO: Função isolada de Morte
    private void Morrer()
    {
        anim.SetTrigger("die");
        rb.linearVelocity = Vector2.zero; // Para de deslizar
        this.enabled = false; // Desliga o script para o jogador não andar morto
    }
}