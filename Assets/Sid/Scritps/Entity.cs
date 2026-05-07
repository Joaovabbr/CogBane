using UnityEngine;
using System.Collections;

public abstract class Entity : MonoBehaviour
{
    [Header("Atributos Definidos pela Classe Filha")]
    public float vidaMaxima;
    public float vidaAtual;
    [Header("Efeitos Visuais")]
    // Arraste o SpriteRenderer do filho para cá no Inspector
    [SerializeField] protected SpriteRenderer spriteRenderer; 
    [SerializeField] protected float tempoParaSumir = 2f;

    protected Animator anim;

    protected virtual void Awake()
    {
        ConfigurarAtributos(); 
        
        vidaAtual = vidaMaxima;
        anim = GetComponent<Animator>();
    }

    protected abstract void ConfigurarAtributos();

    public virtual void TomarDano(float dano)
    {
        vidaAtual -= dano;
        vidaAtual = Mathf.Clamp(vidaAtual, 0, vidaMaxima);

        if (vidaAtual <= 0)
        {
            Morrer();
        }
    }
    

    protected virtual void Morrer()
    {
        if (anim != null) anim.SetTrigger("die");
        
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.bodyType = RigidbodyType2D.Static;
            
        }
        
        Collider2D[] todosColisores = GetComponentsInChildren<Collider2D>();
        foreach (Collider2D col in todosColisores)
        {
            col.enabled = false;
        }
        StartCoroutine(EfeitoPiscarVermelho());
        this.enabled = false;
        Destroy(gameObject, 6f);
        
    }
    protected IEnumerator EfeitoPiscarVermelho()
    {
        // Faz o inimigo piscar algumas vezes (neste exemplo, 4 vezes)
        for (int i = 0; i < 4; i++)
        {
            // Muda a cor do sprite para vermelho
            if (spriteRenderer != null) spriteRenderer.color = Color.red;
            
            // Espera 0.15 segundos
            yield return new WaitForSeconds(0.1f);
            
            // Volta para a cor normal (branco na Unity significa a cor original da imagem)
            if (spriteRenderer != null) spriteRenderer.color = Color.white;
            
            // Espera mais 0.15 segundos antes de repetir o loop
            yield return new WaitForSeconds(0.1f);
        }
    }
}