using UnityEngine;
using System.Collections;

public abstract class Entity : MonoBehaviour
{
    [Header("Atributos Definidos pela Classe Filha")]
    public float vidaMaxima;
    public float vidaAtual;
    
    [Header("Efeitos Visuais")]
    [SerializeField] protected SpriteRenderer spriteRenderer; 
    [SerializeField] protected float tempoParaSumir = 2f;

    [Header("Configurações de dano")]
    protected float tempoUltimoDano = -100f;

    public bool invencible = false;
    
    protected Animator anim;

    protected virtual void Awake()
    {
        anim = GetComponent<Animator>();
        ConfigurarAtributos(); 
        
        // Mantém a vida apenas se não tiver sido carregada do ScriptableObject
        if (vidaAtual <= 0)
        {
            vidaAtual = vidaMaxima;
        }
    }

    protected abstract void ConfigurarAtributos();

    public virtual void TomarDano(float dano, string type)
    {
        if (invencible) return;
        float tempoInvencivel = type == "enemy" ? 0.2f : 0.5f;
        if (Time.time - tempoUltimoDano < tempoInvencivel) 
        {
            return; 
        }
        tempoUltimoDano = Time.time;
        
        vidaAtual -= dano;
        vidaAtual = Mathf.Clamp(vidaAtual, 0, vidaMaxima);
        StartCoroutine(EfeitoPiscarVermelho());
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
        StartCoroutine(EfeitoPiscarMorte());
        //this.enabled = false;
        Destroy(gameObject, 6f);
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
    
    protected IEnumerator EfeitoPiscarMorte()
    {
        while (true) 
        {
            if (spriteRenderer != null) 
                spriteRenderer.color = new Color(1f, 1f, 1f, 0f); 
            
            yield return new WaitForSeconds(0.05f);
            
            if (spriteRenderer != null) 
                spriteRenderer.color = new Color(1f, 1f, 1f, 1f); 
            
            yield return new WaitForSeconds(0.05f);
        }
    }
}