using UnityEngine;
using System.Collections;

public abstract class Entity : MonoBehaviour
{
    [Header("Atributos Definidos pela Classe Filha")]
    public float vidaMaxima;
    public float vidaAtual;

    [Header("Efeitos Visuais")]
    [SerializeField] protected SpriteRenderer spriteRenderer;
    [SerializeField] protected float tempoParaSumir = 0f; // 0 = pisca imediatamente ao morrer

    [Header("Configurações de Dano")]
    protected float tempoUltimoDano = -100f;
    public bool invencible = false;

    protected Animator anim;
    private Coroutine coroutinePiscarVermelho;
    private Coroutine coroutineSequenciaMorte;

    protected virtual void Awake()
    {
        anim = GetComponent<Animator>();

        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();

        ConfigurarAtributos();
        vidaAtual = vidaMaxima; // Sempre garante vida cheia ao iniciar
    }

    protected abstract void ConfigurarAtributos();

    public virtual void TomarDano(float dano, string type)
    {
        if (invencible) return;

        float tempoInvencivel = type == "enemy" ? 0.2f : 0.5f;
        if (Time.time - tempoUltimoDano < tempoInvencivel) return;

        tempoUltimoDano = Time.time;

        vidaAtual -= dano;
        vidaAtual = Mathf.Clamp(vidaAtual, 0, vidaMaxima);

        if (vidaAtual <= 0)
        {
            Morrer();
        }
        else
        {
            // Pisca vermelho apenas se não for o golpe fatal
            if (coroutinePiscarVermelho != null) StopCoroutine(coroutinePiscarVermelho);
            coroutinePiscarVermelho = StartCoroutine(EfeitoPiscarVermelho());
        }
    }

    protected virtual void Morrer()
    {
        // Cancela piscar vermelho caso esteja rodando
        if (coroutinePiscarVermelho != null) StopCoroutine(coroutinePiscarVermelho);
        
        // Reseta a cor antes de iniciar a sequência de morte
        if (spriteRenderer != null) spriteRenderer.color = Color.white;

        if (anim != null) anim.SetTrigger("die");

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.bodyType = RigidbodyType2D.Static;
        }

        Collider2D[] todosColisores = GetComponentsInChildren<Collider2D>();
        foreach (Collider2D col in todosColisores)
            col.enabled = false;

        if (coroutineSequenciaMorte != null) StopCoroutine(coroutineSequenciaMorte);
        coroutineSequenciaMorte = StartCoroutine(SequenciaMorte());
    }

    protected IEnumerator EfeitoPiscarVermelho()
    {
        for (int i = 0; i < 2; i++)
        {
            if (spriteRenderer != null) spriteRenderer.color = Color.red;
            yield return new WaitForSeconds(0.1f);
            if (spriteRenderer != null) spriteRenderer.color = Color.white;
            yield return new WaitForSeconds(0.1f);
        }
    }

    private IEnumerator SequenciaMorte()
    {
        // Aguarda antes de começar a piscar (0 = começa imediatamente)
        if (tempoParaSumir > 0f)
            yield return new WaitForSeconds(tempoParaSumir);

        // Desliga o Animator para soltar o controle da sprite
        if (anim != null) anim.enabled = false;

        float tempoPiscando = 1.5f;
        float cronometro = 0f;
        float velocidadeDoPiscar = 0.07f;
        bool visivel = true;

        while (cronometro < tempoPiscando)
        {
            visivel = !visivel;
            if (spriteRenderer != null)
                spriteRenderer.color = new Color(1f, 1f, 1f, visivel ? 1f : 0f);

            yield return new WaitForSeconds(velocidadeDoPiscar);
            cronometro += velocidadeDoPiscar;
        }

        Destroy(gameObject);
    }
}