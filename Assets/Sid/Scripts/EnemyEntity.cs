using UnityEngine;
using System.Collections;

public abstract class EnemyEntity : Entity
{
    [Header("Efeitos Visuais de Inimigo")]
    [SerializeField] protected SpriteRenderer spriteRenderer;
    [SerializeField] protected float tempoParaSumir = 2f; 

    private Coroutine coroutinePiscarVermelho;
    private Coroutine coroutineSequenciaMorte;

    protected override void Awake()
    {
        base.Awake();
        if (spriteRenderer == null) spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public override void TomarDano(float dano, string type)
    {
        float vidaAntes = vidaAtual;
        base.TomarDano(dano, type);

        if (vidaAtual < vidaAntes && vidaAtual > 0)
        {
            if (coroutinePiscarVermelho != null) StopCoroutine(coroutinePiscarVermelho);
            coroutinePiscarVermelho = StartCoroutine(EfeitoPiscarVermelho());
        }
    }

    protected override void Morrer()
    {
        if (coroutinePiscarVermelho != null) StopCoroutine(coroutinePiscarVermelho);
        if (spriteRenderer != null) spriteRenderer.color = Color.white;

        base.Morrer(); 

      
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
        if (tempoParaSumir > 0f) yield return new WaitForSeconds(tempoParaSumir);

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